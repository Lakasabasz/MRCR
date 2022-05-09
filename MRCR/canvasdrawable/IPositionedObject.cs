using PointInt = System.Drawing.Point;

namespace MRCR.canvasdrawable;

public interface IPositionedObject
{
    PointInt GetPosition();
    void SetPosition(PointInt position, double scale=1);
    bool IsOnPosition(PointInt position, double tolerance = 0, bool cartesian=true);
}