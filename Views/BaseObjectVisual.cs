using System.Windows.Media;
using PhysicsEngineCore.Objects;

namespace PhysicsEngineRender.Views{
    class BaseObjectVisual(IObject baseObjectData) : DrawingVisual {
        public readonly IObject baseObjectData = baseObjectData;
    }
}
