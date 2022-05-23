using System;

namespace MRCR.datastructures;

public enum CoordinatesMode
{
    World = 0,
    Drawing = 1
}

public class UnifiedPoint
{
    public double X { get; set; }
    public double Y { get; set; }
    public CoordinatesMode Mode { get; private set; } = CoordinatesMode.World;
    
    public UnifiedPoint(){}
    
    public UnifiedPoint(int x, int y, CoordinatesMode mode = CoordinatesMode.Drawing)
    {
        X = x;
        Y = y;
        Mode = mode;
    }
    
    public UnifiedPoint(double x, double y, CoordinatesMode mode = CoordinatesMode.World)
    {
        X = x;
        Y = y;
        Mode = mode;
    }

    public void Convert(CoordinatesMode mode, double scale)
    {
        if (Mode > mode)
        {
            X /= scale;
            Y /= scale;
            Mode = mode;
        }
        else if(Mode < mode)
        {
            X *= scale;
            Y *= scale;
            Mode = mode;
        }
    }
    
    public System.Drawing.Point ToDrawingPoint(double scale = 1, bool forceScale = false)
    {
        if (!forceScale && Mode == CoordinatesMode.Drawing)
            return new System.Drawing.Point((int)X, (int)Y);
        return new System.Drawing.Point((int)Math.Round(X * scale), (int)Math.Round(Y*scale));
    }
    
    public System.Windows.Point ToWindowsPoint()
    {
        return new System.Windows.Point(X, Y);
    }

    public UnifiedPoint Move(double x, double y)
    {
        X += x;
        Y += y;
        return this;
    }
    public static bool operator >(UnifiedPoint a, UnifiedPoint b)
    {
        return a.X > b.X && a.Y > b.Y;
    }
    public static bool operator <(UnifiedPoint a, UnifiedPoint b)
    {
        return a.X < b.X && a.Y < b.Y;
    }
    public static bool operator ==(UnifiedPoint? a, UnifiedPoint? b)
    {
        if(a is null && b is null) return true;
        if(a is null || b is null) return false;
        return Math.Abs(a.X - b.X) < 0.001 && Math.Abs(a.Y - b.Y) < 0.001;
    }

    public static bool operator !=(UnifiedPoint? a, UnifiedPoint? b)
    {
        return !(a == b);
    }

    public UnifiedPoint ConvertAsNew(CoordinatesMode drawing, double scale)
    {
        UnifiedPoint up = new UnifiedPoint(X, Y, Mode);
        up.Convert(drawing, scale);
        return up;
    }

    public override string ToString()
    {
        return "(" + X + ", " + Y + "): " + Mode;
    }
}