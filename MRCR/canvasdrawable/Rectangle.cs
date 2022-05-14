using System;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MRCR.canvasdrawable;

using PointInt = System.Drawing.Point;
using SizeInt = System.Drawing.Size;

public class Rectangle: IDrawableProxy
{
    private System.Windows.Shapes.Rectangle _rectangle;
    private PointInt _position;
    
    public Rectangle(SizeInt size, PointInt position, Brush brush, double scale = 1)
    {
        _rectangle = new System.Windows.Shapes.Rectangle()
        {
            Height = size.Height, Width = size.Width,
            Fill = brush, Stroke = Brushes.Transparent, StrokeThickness = 2
        };
        SetPosition(position, scale);
    }

    public Point GetPosition() => _position;
    public void SetPosition(Point position, double scale = 1)
    {
        _position = position;
        Canvas.SetTop(_rectangle, _position.Y*scale);
        Canvas.SetLeft(_rectangle, _position.X*scale);
    }

    public bool IsOnPosition(Point position, double tolerance = 0, bool cartesian = true)
    {
        if (cartesian) return Math.Pow(_position.X - position.X, 2) + Math.Pow(_position.Y - position.Y, 2) <= tolerance;
        return Math.Abs(_position.X - position.X) + Math.Abs(_position.Y - position.Y) <= tolerance;
    }
    
    public bool IsSelected
    {
        get => _rectangle.Stroke == Brushes.Transparent;
        set
        {
            if (value) _rectangle.Stroke = Brushes.DodgerBlue;
            else _rectangle.Stroke = Brushes.Transparent;
        }
    }

    public Shape GetDrawable() => _rectangle;
}