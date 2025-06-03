using System.Windows;
using System.Windows.Media;

namespace PhysicsEngineCore{
    public class VisualControl : FrameworkElement{
        private readonly VisualCollection _visuals;

        public VisualControl(){
            _visuals = new VisualCollection(this);
        }

        public void AddVisual(Visual visual){
            _visuals.Add(visual);
        }

        public void RemoveVisual(Visual visual){
            if(_visuals.Contains(visual)){
                _visuals.Remove(visual);
            }
        }

        public void ClearVisuals(){
            _visuals.Clear();
        }

        protected override int VisualChildrenCount{
            get{
               return _visuals.Count;
            }
        }

        protected override Visual GetVisualChild(int index){
            return _visuals[index];
        }
    }
}
