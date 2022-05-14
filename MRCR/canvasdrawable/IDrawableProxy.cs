using System.Windows.Shapes;

namespace MRCR.canvasdrawable;

public interface IDrawableProxy : IPositionedObject
{
    bool IsSelected { get; set; }
    Shape GetDrawable();
}