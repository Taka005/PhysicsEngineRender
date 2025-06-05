using System.Windows;
using System.Windows.Media;
using PhysicsEngineCore.Objects;

namespace PhysicsEngineRender.Views{
    class BaseObjectVisual(IObject baseObjectData) : DrawingVisual {
        private readonly IObject baseObjectData = baseObjectData;
        private readonly Pen vectorPen = new Pen(Brushes.Black, 1);

        public void DrawVector(DrawingContext context){
            Point startPoint = new Point(this.baseObjectData.position.X, this.baseObjectData.position.Y);
            Point endPoint = new Point(this.baseObjectData.position.X + this.baseObjectData.velocity.X, this.baseObjectData.position.X + this.baseObjectData.velocity.Y);

            context.DrawLine(vectorPen, startPoint, endPoint);
        }
    }
}
