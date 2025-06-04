using System.Windows;
using System.Windows.Media;

namespace PhysicsEngineRender{
    public class VisualControl : FrameworkElement{
        private readonly VisualCollection visuals;

        public VisualControl(){
            visuals = new VisualCollection(this);
        }

        public void AddVisual(Visual visual){
            visuals.Add(visual);
        }

        public void RemoveVisual(Visual visual){
            if(visuals.Contains(visual)){
                visuals.Remove(visual);
            }
        }

        public void ClearVisuals(){
            visuals.Clear();
        }

        protected override int VisualChildrenCount{
            get{
               return visuals.Count;
            }
        }

        protected override Visual GetVisualChild(int index){
            return visuals[index];
        }
    }
}
