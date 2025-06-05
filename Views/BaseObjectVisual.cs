using System.Windows;
using System.Windows.Media;
using PhysicsEngineCore.Objects;

namespace PhysicsEngineRender.Views{
    class BaseObjectVisual(IObject vectorObjectData) : DrawingVisual {
        private readonly IObject vectorObjectData = vectorObjectData;
        private readonly Pen vectorPen = new Pen(Brushes.Black, 1);

        public void DrawVector(DrawingContext context){
            Point startPoint = new Point(this.vectorObjectData.position.X, this.vectorObjectData.position.Y);
            Point endPoint = new Point(this.vectorObjectData.position.X + this.vectorObjectData.velocity.X, this.vectorObjectData.position.X + this.vectorObjectData.velocity.Y);

            context.DrawLine(vectorPen, startPoint, endPoint);
        }
    }
}
