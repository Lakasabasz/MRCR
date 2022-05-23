using System;
using MRCR.datastructures;
using PointInt = System.Drawing.Point;

namespace MRCR.canvasdrawable;

public interface IPositionedObject
{
    UnifiedPoint GetPosition();
    void SetPosition(UnifiedPoint position, double scale=1);
    [Obsolete("Use GetPosition(UnifiedPoint, double) instead")]
    void SetPosition(PointInt position, double scale=1);
    [Obsolete("Use IsOnPosition(UnifiedPoint, double, bool) instead")]
    bool IsOnPosition(PointInt position, double tolerance = 0, bool cartesian=true);
    bool IsOnPosition(UnifiedPoint position, double tolerance = 0.0001, bool cartesian=true);
}