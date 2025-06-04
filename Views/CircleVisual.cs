using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using PhysicsEngineCore.Objects;
using PhysicsEngineCore.Utils;

namespace PhysicsEngineRender.Views{
    class CircleVisual : DrawingVisual{
        private readonly Circle objectData;
        private Brush brush;
        private Pen pen;
        private readonly Dispatcher dispatcher;

        public CircleVisual(Circle objectData) {
            this.objectData = objectData;
            this.brush = Utility.ParseColor(objectData.color);
            this.pen = new Pen(this.brush,1);
            this.dispatcher = Application.Current.Dispatcher;
        }

        public void UpdateDrawing() {
            if(this.dispatcher.Thread != Thread.CurrentThread){
                this.dispatcher.InvokeAsync(()=> this.Draw());
            }else{
                this.Draw();
            }
        }

        private void Draw() {
            DrawingContext context = this.RenderOpen();

            this.brush = Utility.ParseColor(this.objectData.color);
            this.pen = new Pen(this.brush, 1);

            context.DrawEllipse(
                this.brush,
                this.pen,
                new Point(this.objectData.position.X, this.objectData.position.Y),
                this.objectData.radius,
                this.objectData.radius
            );
        }
    }
}
