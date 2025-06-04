using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using PhysicsEngineCore.Objects;
using PhysicsEngineCore.Utils;

namespace PhysicsEngineRender.Views {
    class LineVisual : DrawingVisual {
        private readonly Line groundData;
        private Brush brush;
        private Pen pen;
        private readonly Dispatcher dispatcher;

        public LineVisual(Line groundData) {
            this.groundData = groundData;
            this.brush = Utility.ParseColor(groundData.color);
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

            this.brush = Utility.ParseColor(this.groundData.color);
            this.pen = new Pen(this.brush, this.groundData.width);

            context.DrawLine(
                this.pen,
                new Point(this.groundData.start.X, this.groundData.start.Y),
                new Point(this.groundData.end.X, this.groundData.end.Y)
            );

            context.DrawEllipse(
                this.brush,
                null,
                new Point(this.groundData.start.X, this.groundData.start.Y),
                this.groundData.width / 2,
                this.groundData.width / 2
            );

            context.DrawEllipse(
                this.brush,
                null,
                new Point(this.groundData.end.X, this.groundData.end.Y),
                this.groundData.width / 2,
                this.groundData.width / 2
            );
        }
    }
}
