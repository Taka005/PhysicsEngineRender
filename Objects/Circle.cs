using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PhysicsEngineRender.Objects{
    public class Circle : Control{
        protected override void OnRender(DrawingContext context){
            base.OnRender(context);

            context.DrawRectangle(Brushes.Crimson, new Pen(Brushes.Crimson, 1), new Rect(0, 0, Width, Height));
        }
    }
}
