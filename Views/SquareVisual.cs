using System.Windows;
using System.Windows.Media;
using PhysicsEngineCore.Objects;

namespace PhysicsEngineRender.Views {
    class SquareVisual : BaseObjectVisual {
        private readonly Square objectData;
        private Brush brush;
        private Pen pen;

        public SquareVisual(Square objectData):base(objectData) {
            this.objectData = objectData;
            this.brush = Utility.ParseColor(objectData.color);
            this.pen = new Pen(this.brush, 1);
        }

        public void Draw() {
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

            context.Close();
        }
    }
}
