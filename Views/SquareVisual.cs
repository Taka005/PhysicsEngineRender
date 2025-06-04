using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using PhysicsEngineCore.Objects;
using PhysicsEngineCore.Utils;

namespace PhysicsEngineRender.Views {
    class SquareVisual : DrawingVisual {
        private readonly Square objectData;
        private Brush brush;
        private Pen pen;
        private readonly Dispatcher dispatcher;

        public SquareVisual(Square objectData) {
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
            this.pen = new Pen(this.brush, this.objectData.size/2);

            Entity start = this.objectData.entities[0];
            Entity end = this.objectData.entities[3];

            this.objectData.entities.ForEach(entity => {
                context.DrawEllipse(
                    this.brush,
                    null,
                    new Point(entity.position.X, entity.position.Y),
                    entity.radius,
                    entity.radius
                );

                if(
                    start.id == entity.id &&
                    end.id == entity.id
                ) return;

                context.DrawLine(
                    this.pen,
                    new Point(start.position.X, start.position.Y),
                    new Point(entity.position.X, entity.position.Y)
                );

                context.DrawLine(
                    this.pen,
                    new Point(end.position.X, end.position.Y),
                    new Point(entity.position.X, entity.position.Y)
                );
            });
        }
    }
}
