using PhysicsEngineCore.Objects;
using PhysicsEngineRender.Views;
using System.Windows.Media;

namespace PhysicsEngineRender{
    public class Render {
        public VisualCollection visuals;

        private readonly Dictionary<string, DrawingVisual> objectVisuals = [];

        public Render() {
            this.visuals = new VisualCollection(null);
        }

        /// <summary>
        /// レンダリング対象のVisualCollectionを設定します
        /// このメソッドはUIスレッドで呼び出される必要があります
        /// </summary>
        public void SetVisualCollection(VisualCollection visualCollection){
            this.visuals = visualCollection;

            foreach(var visual in this.objectVisuals.Values){
                this.visuals.Add(visual);
            }
        }


        /// <summary>
        /// 物理エンジンのオブジェクトデータを受け取り、描画を更新します
        /// このメソッドはUIスレッドで呼び出される必要があります
        /// </summary>
        /// <param name="objects">描画するIObjectのリスト</param>
        public void UpdateAndDraw(List<IObject> objects) {
            HashSet<string> currentObjectIds = [.. objects.Select(o => o.id)];
            List<string>? visualsToRemove = this.objectVisuals.Keys.Where(id => !currentObjectIds.Contains(id)).ToList();

            visualsToRemove.ForEach(id =>{
                this.visuals.Remove(this.objectVisuals[id]);
                this.objectVisuals.Remove(id);
            });

            objects.ForEach(obj => {
                if(!this.objectVisuals.ContainsKey(obj.id)) {
                    DrawingVisual? newVisual = this.CreateVisualForObject(obj);

                    if(newVisual != null) {
                        this.objectVisuals.Add(obj.id, newVisual);
                        this.visuals.Add(newVisual);
                    }
                }

                if(this.objectVisuals.TryGetValue(obj.id, out DrawingVisual? visual)) {
                    if(visual is CircleVisual circleVisual) {
                        circleVisual.UpdateDrawing();
                    } else if(visual is SquareVisual squareVisual) {
                        squareVisual.UpdateDrawing();
                    } else if(visual is RopeVisual ropeVisual){
                        ropeVisual.UpdateDrawing();
                    }
                }
            });
        }

        /// <summary>
        /// オブジェクトの種類に基づいて適切なDrawingVisualを作成
        /// </summary>
        private DrawingVisual? CreateVisualForObject(IObject obj) {
            if(obj is Circle circle) {
                return new CircleVisual(circle);
            }else if(obj is Rope rope) {
                return new RopeVisual(rope);
            }else if(obj is Square square) {
                return new SquareVisual(square);
            }

            return null;
        }

        /// <summary>
        /// 地面の種類に基づいて適切なDrawingVisualを作成
        /// </summary>
        private DrawingVisual? CreateVisualForGround(IGround obj) {
            if(obj is Line line) {
                return new LineVisual(line);
            }else if(obj is Curve curve) {
                return new CurveVisual(curve);
            }

            return null;
        }
    }
}
