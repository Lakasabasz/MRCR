using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

using PointInt = System.Drawing.Point;
using SizeInt = System.Drawing.Size;

namespace MRCR.canvasdrawable;

public class Ellipse: IDrawableProxy
{
    private PointInt _position;
    private System.Windows.Shapes.Ellipse _ellipse;

    public Ellipse(SizeInt size, PointInt position, Brush color, double scale = 1)
    {
        _ellipse = new System.Windows.Shapes.Ellipse()
        {
            Width = size.Width, Height = size.Height,
            Fill = color
        };
        SetPosition(position, scale);
    }

    public PointInt GetPosition() => _position;
    public void SetPosition(PointInt position, double scale = 1)
    {
        _position = position;
        Canvas.SetTop(_ellipse, _position.Y*scale);
        Canvas.SetLeft(_ellipse, _position.X*scale);
    }

    public bool IsOnPosition(PointInt position, double tolerance = 0, bool cartesian = true)
    {
        if (cartesian) return Math.Pow(_position.X - position.X, 2) + Math.Pow(_position.Y - position.Y, 2) <= tolerance;
        return Math.Abs(_position.X - position.X) + Math.Abs(_position.Y - position.Y) <= tolerance;
    }
    public bool IsSelected
    {
        get => _ellipse.Stroke == Brushes.Transparent;
        set
        {
            if (value) _ellipse.Stroke = Brushes.DodgerBlue;
            else _ellipse.Stroke = Brushes.Transparent;
        }
    }
    public Shape GetDrawable() => _ellipse;
}