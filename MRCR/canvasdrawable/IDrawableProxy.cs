using System.Collections.Generic;
using System.Windows.Shapes;
using MRCR.datastructures;

namespace MRCR.canvasdrawable;

public enum ScalePolicy
{
    Fixed,
    OnlyPosition,
    OnlySize,
    Both
}

public interface IDrawableProxy : IPositionedObject
{
    bool IsSelected { get; set; }
    Shape GetDrawable();
    List<UnifiedPoint> GetBorderPoints();
    bool IsInside(UnifiedPoint point);
    void UpdateScale(double scale);
    void SetSize(UnifiedPoint unifiedPoint, double canvasScale);
}