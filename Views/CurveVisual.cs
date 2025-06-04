using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using PhysicsEngineCore.Objects;
using PhysicsEngineCore.Utils;

namespace PhysicsEngineRender.Views {
    class CurveVisual : DrawingVisual {
        private readonly Curve groundData;
        private Brush brush;
        private Pen pen;
        private readonly Dispatcher dispatcher;

        public CurveVisual(Curve groundData) {
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

            double startAngle = Curve.NormalizeAngle(Math.Atan2(this.groundData.start.Y - this.groundData.center.Y, this.groundData.start.X - this.groundData.center.X));
            double endAngle = Curve.NormalizeAngle(Math.Atan2(this.groundData.end.Y - this.groundData.center.Y, this.groundData.end.X - this.groundData.center.X));
            double midAngle = Curve.NormalizeAngle(Math.Atan2(this.groundData.middle.Y - this.groundData.center.Y, this.groundData.middle.X - this.groundData.center.X));
            bool clockwise = (startAngle > endAngle) ? (midAngle > startAngle || midAngle < endAngle) : (midAngle > startAngle && midAngle < endAngle);

            StreamGeometry arcGeometry = new StreamGeometry();
            StreamGeometryContext sgc = arcGeometry.Open();

            sgc.BeginFigure(
                new Point(this.groundData.center.X + this.groundData.radius * Math.Cos(startAngle), this.groundData.center.Y + this.groundData.radius * Math.Sin(startAngle)),
                false,
                false
            );

            sgc.ArcTo(
                new Point(this.groundData.center.X + this.groundData.radius * Math.Cos(endAngle), this.groundData.center.Y + this.groundData.radius * Math.Sin(endAngle)),
                new Size(this.groundData.radius, this.groundData.radius),
                0,
                clockwise,
                SweepDirection.Clockwise,
                true,
                true
            );

            context.DrawGeometry(null,this.pen, arcGeometry);

            context.DrawEllipse(
                brush,
                null,
                new Point(this.groundData.start.X, this.groundData.start.Y),
                this.groundData.width / 2, this.groundData.width / 2
            );

            context.DrawEllipse(
                brush,
                null,
                new Point(this.groundData.end.X, this.groundData.end.Y),
                this.groundData.width / 2, this.groundData.width / 2
            );
        }
    }
}
