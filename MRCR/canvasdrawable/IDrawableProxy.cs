using System.Windows.Shapes;

namespace MRCR.canvasdrawable;

public interface IDrawableProxy : IPositionedObject
{
    Shape GetDrawable();
}