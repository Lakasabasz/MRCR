using System;

namespace MRCR.datastructures;

public class UnifiedPoint
{
    public double X { get; set; }
    public double Y { get; set; }
    
    public UnifiedPoint(){}
    
    public UnifiedPoint(int x, int y)
    {
        X = x;
        Y = y;
    }
    
    public UnifiedPoint(double x, double y)
    {
        X = x;
        Y = y;
    }
    
    public System.Drawing.Point ToDrawingPoint()
    {
        return new System.Drawing.Point((int)Math.Round(X), (int)Math.Round(Y));
    }
    
    public System.Windows.Point ToWindowsPoint()
    {
        return new System.Windows.Point(X, Y);
    }

    public UnifiedPoint Move(int x, int y)
    {
        X += x;
        Y += y;
        return this;
    }
}