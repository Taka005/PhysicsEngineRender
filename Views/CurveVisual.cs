﻿using System.Windows;
using System.Windows.Media;
using PhysicsEngineCore.Objects;

namespace PhysicsEngineRender.Views {
    class CurveVisual : DrawingVisual {
        private readonly Curve groundData;
        private Brush brush;
        private Pen pen;

        public CurveVisual(Curve groundData) {
            this.groundData = groundData;
            this.brush = Utility.ParseColor(this.groundData.color);
            this.pen = new Pen(this.brush, this.groundData.width);
        }

        public void Draw() {
            DrawingContext context = this.RenderOpen();

            this.brush = Utility.ParseColor(this.groundData.color);
            this.pen = new Pen(this.brush, this.groundData.width);

            double startAngle = Curve.NormalizeAngle(Math.Atan2(this.groundData.start.Y - this.groundData.center.Y, this.groundData.start.X - this.groundData.center.X));
            double endAngle = Curve.NormalizeAngle(Math.Atan2(this.groundData.end.Y - this.groundData.center.Y, this.groundData.end.X - this.groundData.center.X));
            double midAngle = Curve.NormalizeAngle(Math.Atan2(this.groundData.middle.Y - this.groundData.center.Y, this.groundData.middle.X - this.groundData.center.X));
            bool clockwise = (startAngle > endAngle) ?
                (midAngle > startAngle || midAngle < endAngle) :
                (midAngle > startAngle && midAngle < endAngle);

            bool isLargeArc = clockwise ? 
                (endAngle <= startAngle) || (Math.Abs(endAngle - startAngle) > Math.PI) : 
                (endAngle >= startAngle) || (Math.Abs(endAngle - startAngle) > Math.PI);

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
                isLargeArc,
                clockwise ? SweepDirection.Clockwise : SweepDirection.Counterclockwise,
                true,
                false
            );

            context.DrawGeometry(null, this.pen, arcGeometry);

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

            sgc.Close();
            context.Close();
        }
    }
}
