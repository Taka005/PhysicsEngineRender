using PhysicsEngineCore.Objects;
using PhysicsEngineRender.Views;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;

namespace PhysicsEngineRender{
    public class Render : FrameworkElement {
        private readonly VisualCollection visuals;
        private readonly Dictionary<string, DrawingVisual> objectVisuals = [];

        public Render() {
            this.visuals = new VisualCollection(this);
        }

        /// <summary>
        /// 物理エンジンのオブジェクトデータを受け取り、描画を更新します
        /// このメソッドはUIスレッドで呼び出される必要があります
        /// </summary>
        /// <param name="objects">描画するオブジェクトのリスト</param>
        public void DrawObject(List<IObject> objects) {
            Debug.WriteLine(this.visuals.Count);
            Debug.WriteLine(objects);
            HashSet<string> currentObjectIds = [.. objects.Select(o => o.id)];
            List<string>? visualsToRemove = [.. this.objectVisuals.Keys.Where(id => !currentObjectIds.Contains(id))];

            foreach(string id in visualsToRemove) {
                this.visuals.Remove(this.objectVisuals[id]);
                this.objectVisuals.Remove(id);
            }

            foreach(IObject obj in objects) {
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
                    } else if(visual is RopeVisual ropeVisual) {
                        ropeVisual.UpdateDrawing();
                    }
                }
            }
        }

        /// <summary>
        /// 物理エンジンのオブジェクトデータを受け取り、描画を更新します
        /// このメソッドはUIスレッドで呼び出される必要があります
        /// </summary>
        /// <param name="grounds">描画する地面のリスト</param>
        public void DrawGround(List<IGround> grounds) {
            HashSet<string> currentGrounds = [.. grounds.Select(o => o.id)];
            List<string>? visualsToRemove = [.. this.objectVisuals.Keys.Where(id => !currentGrounds.Contains(id))];

            foreach(string id in visualsToRemove) {
                this.visuals.Remove(this.objectVisuals[id]);
                this.objectVisuals.Remove(id);
            }

            foreach(IGround ground in grounds) {
                if(!this.objectVisuals.ContainsKey(ground.id)) {
                    DrawingVisual? newVisual = this.CreateVisualForGround(ground);

                    if(newVisual != null) {
                        this.objectVisuals.Add(ground.id, newVisual);
                        this.visuals.Add(newVisual);
                    }
                }

                if(this.objectVisuals.TryGetValue(ground.id, out DrawingVisual? visual)) {
                    if(visual is LineVisual lineVisual) {
                        lineVisual.UpdateDrawing();
                    } else if(visual is CurveVisual curveVisual) {
                        curveVisual.UpdateDrawing();
                    }
                }
            }
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

        protected override int VisualChildrenCount {
            get {
                return this.visuals.Count;
            }
        }

        protected override Visual GetVisualChild(int index) {
            return this.visuals[index];
        }
    }
}
