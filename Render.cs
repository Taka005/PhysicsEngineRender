﻿using PhysicsEngineCore.Objects;
using PhysicsEngineRender.Views;
using System.Windows;
using System.Windows.Media;

namespace PhysicsEngineRender{
    /// <summary>
    /// 物理エンジンのレンダークラス
    /// </summary>
    public class Render : FrameworkElement {
        public bool isDebugMode = false;
        private readonly VisualCollection visuals;
        private readonly Dictionary<string, DrawingVisual> objectVisuals = [];
        private readonly Dictionary<string, DrawingVisual> groundVisuals = [];
        private readonly OverlayVisual overlayVisual = new OverlayVisual();

        public Render() {
            this.visuals = new VisualCollection(this) {
                this.overlayVisual
            };
        }

        /// <summary>
        /// 物理エンジンのオブジェクトデータを受け取り、描画を更新します
        /// このメソッドはUIスレッドで呼び出される必要があります
        /// </summary>
        /// <param name="objects">描画するオブジェクトのリスト</param>
        public void DrawObject(List<IObject> objects) {
            HashSet<string> currentObjectIds = [.. objects.Select(o => o.id)];
            List<string>? visualsToRemove = [.. this.objectVisuals.Keys.Where(id => !currentObjectIds.Contains(id))];
            List<VectorData> vectors = [];

            foreach(string id in visualsToRemove) {
                this.visuals.Remove(this.objectVisuals[id]);
                this.objectVisuals.Remove(id);
            }

            foreach(IObject obj in objects) {
                if(!this.objectVisuals.ContainsKey(obj.id)) {
                    DrawingVisual? newVisual = this.CreateVisualForObject(obj);

                    if(newVisual != null) {
                        this.objectVisuals.Add(obj.id, newVisual);
                        this.visuals.Insert(0,newVisual);
                    }
                }

                if(this.objectVisuals.TryGetValue(obj.id, out DrawingVisual? visual)) {
                    if(visual is CircleVisual circleVisual) {
                        circleVisual.Draw();
                    } else if(visual is SquareVisual squareVisual) {
                        squareVisual.Draw();
                    } else if(visual is RopeVisual ropeVisual) {
                        ropeVisual.Draw();
                    }

                    if(this.isDebugMode && visual is BaseObjectVisual baseVisual) {
                        vectors.Add(new VectorData(
                            baseVisual.baseObjectData.position,
                            baseVisual.baseObjectData.velocity
                        ));
                    }
                }
            }

            if(this.isDebugMode){
                this.overlayVisual.UpdateVectors(vectors);
            }else{
                this.overlayVisual.Clear();
            }
        }

        /// <summary>
        /// 物理エンジンのオブジェクトデータを受け取り、描画を更新します
        /// このメソッドはUIスレッドで呼び出される必要があります
        /// </summary>
        /// <param name="grounds">描画する地面のリスト</param>
        public void DrawGround(List<IGround> grounds) {
            HashSet<string> currentGrounds = [.. grounds.Select(o => o.id)];
            List<string>? visualsToRemove = [.. this.groundVisuals.Keys.Where(id => !currentGrounds.Contains(id))];

            foreach(string id in visualsToRemove) {
                this.visuals.Remove(this.groundVisuals[id]);
                this.groundVisuals.Remove(id);
            }

            foreach(IGround ground in grounds) {
                if(!this.groundVisuals.ContainsKey(ground.id)) {
                    DrawingVisual? newVisual = this.CreateVisualForGround(ground);

                    if(newVisual != null) {
                        this.groundVisuals.Add(ground.id, newVisual);
                        this.visuals.Insert(0,newVisual);
                    }
                }

                if(this.groundVisuals.TryGetValue(ground.id, out DrawingVisual? visual)) {
                    if(visual is LineVisual lineVisual) {
                        lineVisual.Draw();
                    } else if(visual is CurveVisual curveVisual) {
                        curveVisual.Draw();
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

        /// <summary>
        /// この要素の子ビジュアルの数を取得します
        /// </summary>
        protected override int VisualChildrenCount {
            get {
                return this.visuals.Count;
            }
        }

        /// <summary>
        /// 指定されたインデックスの子ビジュアルを取得します
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        protected override Visual GetVisualChild(int index) {
            return this.visuals[index];
        }
    }
}
