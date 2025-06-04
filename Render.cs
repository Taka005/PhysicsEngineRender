using System.Diagnostics;
using System.Windows;

namespace PhysicsEngineRender{
    public class Render : FrameworkElement{
        public readonly VisualControl control = new VisualControl();
        private Stopwatch stopwatch = new Stopwatch();
        private TimeSpan _lastRenderTime = TimeSpan.Zero;
        private TimeSpan _lastFpsUpdateTime = TimeSpan.Zero;
        private int _frameCount = 0;
        private double _fps = 0;
        private int _targetFps;

        public Render(int targetFps = 60){
            this._targetFps = CheckTargetFps(targetFps);

             this.stopwatch.Start();
        }

        public int fps{
            get{
                return (int)this._fps;
            }
        }

         public int targetFps{
            get{
                return this._targetFps;
            }
            set{
                this._targetFps = CheckTargetFps(value);
            }
        }

        public void OnRendering(object? sender,EventArgs e){
            TimeSpan currentTime = this.stopwatch.Elapsed;
            double deltaTime = (currentTime - _lastRenderTime).TotalSeconds;

            if(deltaTime < 1.0 / this._targetFps) return;

            _frameCount++;

            if((currentTime - _lastFpsUpdateTime).TotalSeconds >= 1){
                _fps = _frameCount / (currentTime - _lastFpsUpdateTime).TotalSeconds;
                _lastFpsUpdateTime = currentTime;
                _frameCount = 0;
            }

            _lastRenderTime = currentTime;
        }

        private static int CheckTargetFps(int targetFps){
            if(targetFps < 0) throw new Exception("目標FPSは0以上である必要があります");

            return targetFps;
        }
    }
}
