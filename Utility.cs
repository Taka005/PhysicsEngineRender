using System.Windows.Media;

namespace PhysicsEngineRender {
    public static class Utility {
        public static Brush ParseColor(string colorString) {
            Brush fillBrush;
            try {
                BrushConverter brushConverter = new BrushConverter();
                Brush? convertedBrush = brushConverter.ConvertFromString(colorString) as Brush;

                fillBrush = convertedBrush ?? Brushes.Transparent;
            } catch {
                fillBrush = Brushes.Transparent;
            }

            return fillBrush;
        }
    }
}
