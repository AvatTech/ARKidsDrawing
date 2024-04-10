using Sketch.Modification.Enum;

namespace Sketch.Modification.Abstraction
{
    public abstract class BaseSketchModifier
    {
        public abstract ModificationType modificationType { get; }
    }
}