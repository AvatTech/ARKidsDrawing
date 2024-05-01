using Sketches.Modification.Enum;

namespace Sketches.Modification.Abstraction
{
    public abstract class BaseSketchModifier
    {
        public abstract ModificationType modificationType { get; }
    }
}