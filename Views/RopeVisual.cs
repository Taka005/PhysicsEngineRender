using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using PhysicsEngineCore.Objects;
using PhysicsEngineCore.Utils;

namespace PhysicsEngineRender.Views {
    class RopeVisual : DrawingVisual {
        private readonly Rope objectData;
        private Brush brush;
        private Pen pen;
        private readonly Dispatcher dispatcher;

        public RopeVisual(Rope objectData) {
            this.objectData = objectData;
            this.brush = Utility.ParseColor(objectData.color);
            this.pen = new Pen(this.brush, 1);
            this.dispatcher = Application.Current.Dispatcher;
        }

        public void UpdateDrawing() {
            if(this.dispatcher.Thread != Thread.CurrentThread) {
                this.dispatcher.InvokeAsync(() => this.Draw());
            } else {
                this.Draw();
            }
        }

        private void Draw() {
            DrawingContext context = this.RenderOpen();

            this.brush = Utility.ParseColor(this.objectData.color);
            this.pen = new Pen(this.brush, this.objectData.width);

            Entity? target = null;

            this.objectData.entities.ForEach(entity => {
                if(target != null) {
                    context.DrawLine(
                        this.pen,
                        new Point(entity.position.X, entity.position.Y),
                        new Point(target.position.X, target.position.Y)
                    );

                    context.DrawEllipse(
                        this.brush,
                        null,
                        new Point(entity.position.X, entity.position.Y),
                        entity.radius,
                        entity.radius
                    );
                }

                if(this.objectData.entities.IndexOf(entity) == this.objectData.entities.Count - 1) {
                    context.DrawEllipse(
                        this.brush,
                        null,
                        new Point(entity.position.X, entity.position.Y),
                        entity.radius,
                        entity.radius
                    );
                }

                target = entity;
            });
        }
    }
}