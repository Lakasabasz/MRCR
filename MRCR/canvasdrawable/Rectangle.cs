using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using MRCR.datastructures;

namespace MRCR.canvasdrawable;

using PointInt = System.Drawing.Point;
using SizeInt = System.Drawing.Size;

public class Rectangle: IDrawableProxy
{
    private System.Windows.Shapes.Rectangle _rectangle;
    private UnifiedPoint _position;
    private UnifiedPoint _size;
    private double _scale;
    private ScalePolicy _scalePolicy;
    
    [Obsolete("Use the constructor that takes a UnifiedPoint instead")]
    public Rectangle(SizeInt renderPixelSize, PointInt renderPixelPosition, Brush brush, double scale = 1, ScalePolicy scalePolicy = ScalePolicy.Fixed)
    {
        _rectangle = new System.Windows.Shapes.Rectangle()
        {
            Height = renderPixelSize.Height, Width = renderPixelSize.Width,
            Fill = brush, Stroke = Brushes.Transparent, StrokeThickness = 2
        };
        _size = new UnifiedPoint(renderPixelSize.Width, renderPixelSize.Height, CoordinatesMode.Drawing);
        _scale = scale;
        _scalePolicy = ScalePolicy.Both;
        _position = new UnifiedPoint(renderPixelPosition.X, renderPixelPosition.Y, CoordinatesMode.Drawing);
        SetPosition(renderPixelPosition, scale);
        _scalePolicy = scalePolicy;
    }

    public Rectangle(UnifiedPoint size, UnifiedPoint position, Brush brush, double scale = 1,
        ScalePolicy scalePolicy = ScalePolicy.OnlyPosition)
    {
        if(scalePolicy is ScalePolicy.Both or ScalePolicy.OnlySize)
        {
            size.Convert(CoordinatesMode.World, scale);
        }
        _size = size;

        if (scalePolicy is ScalePolicy.Both or ScalePolicy.OnlyPosition)
        {
            position.Convert(CoordinatesMode.World, scale);
        }
        _position = position;
        
        _rectangle = new System.Windows.Shapes.Rectangle()
        {
            Height = size.Y, Width = size.X,
            Fill = brush, Stroke = Brushes.Transparent, StrokeThickness = 2
        };
        _scale = scale;
        _scalePolicy = scalePolicy;
    }

    public UnifiedPoint GetPosition() => _position;
    
    public void SetPosition(UnifiedPoint position, double scale = 1)
    {
        if (_scalePolicy is ScalePolicy.Fixed or ScalePolicy.OnlySize)
        {
            position.Convert(CoordinatesMode.Drawing, scale);
            _position = position;
            Canvas.SetTop(_rectangle, _position.Y);
            Canvas.SetLeft(_rectangle, _position.X);
            return;
        }
        position.Convert(CoordinatesMode.World, scale);
        _position = position;
    }
    
    [Obsolete("Use SetPosition(UnifiedPoint, double) instead")]
    public void SetPosition(PointInt position, double scale = 1)
    {
        SetPosition(new UnifiedPoint(position.X, position.Y), scale);
    }

    public bool IsOnPosition(PointInt position, double tolerance = 0, bool cartesian = true)
    {
        throw new NotImplementedException();
    }

    public bool IsOnPosition(UnifiedPoint position, double tolerance = 0.0001, bool cartesian = true)
    {
        if(position.Mode != _position.Mode) throw new ArgumentException(
            $"Position must be in the same mode as the rectangle, rescale it first to mode {_position.Mode}");
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

    public Shape GetDrawable()
    {
        if(_scalePolicy is ScalePolicy.Both or ScalePolicy.OnlyPosition)
        {
            UnifiedPoint drawing = _position.ConvertAsNew(CoordinatesMode.Drawing, _scale);
            Canvas.SetTop(_rectangle, drawing.Y);
            Canvas.SetLeft(_rectangle, drawing.X);
        }
        if(_scalePolicy is ScalePolicy.Both or ScalePolicy.OnlySize)
        {
            UnifiedPoint drawing = _size.ConvertAsNew(CoordinatesMode.Drawing, _scale);
            _rectangle.Height = drawing.Y;
            _rectangle.Width = drawing.X;
        }

        return _rectangle;
    }
    public List<UnifiedPoint> GetBorderPoints()
    {
        return new List<UnifiedPoint>
        {
            _position,
            new (_position.X + _rectangle.Width, _position.Y, _position.Mode),
            new (_position.X + _rectangle.Width, _position.Y + _rectangle.Height, _position.Mode),
            new (_position.X, _position.Y + _rectangle.Height, _position.Mode)
        };
    }

    public bool IsInside(UnifiedPoint point)
    {
        UnifiedPoint converted = point.ConvertAsNew(_position.Mode, _scale);
        return  converted.X >= _position.X && converted.X <= _position.X + _rectangle.Width &&
                converted.Y >= _position.Y && converted.Y <= _position.Y + _rectangle.Height;
    }

    public void UpdateScale(double scale) => _scale = scale;
    public void SetSize(UnifiedPoint size, double canvasScale)
    {
        if(size.X < 0 || size.Y < 0) throw new ArgumentException("Size must be positive");
        if (_scalePolicy is ScalePolicy.Both or ScalePolicy.OnlySize)
        {
            size.Convert(CoordinatesMode.World, canvasScale);
            _size = size;
            return;
        }
        size.Convert(CoordinatesMode.Drawing, canvasScale);
        _size = size;
        _rectangle.Height = size.Y;
        _rectangle.Width = size.X;
    }

    public void DrawBetween(UnifiedPoint a, UnifiedPoint b, double scale)
    {
        a.Convert(CoordinatesMode.Drawing, scale);
        b.Convert(CoordinatesMode.Drawing, scale);
        if (a < b)
        {
            SetPosition(a, scale);
            a.Convert(CoordinatesMode.Drawing, scale);
            b.Convert(CoordinatesMode.Drawing, scale);
            SetSize(new UnifiedPoint(b.X - a.X, b.Y - a.Y, CoordinatesMode.Drawing), scale);
            return;
        }
        if (a > b)
        {
            SetPosition(b, scale);
            a.Convert(CoordinatesMode.Drawing, scale);
            b.Convert(CoordinatesMode.Drawing, scale);
            SetSize(new UnifiedPoint(a.X - b.X, a.Y - b.Y, CoordinatesMode.Drawing), scale);
            return;
        }
        UnifiedPoint c = new UnifiedPoint(a.X, b.Y, CoordinatesMode.Drawing);
        UnifiedPoint d = new UnifiedPoint(b.X, a.Y, CoordinatesMode.Drawing);
        if (c < d)
        {
            SetPosition(c, scale);
            c.Convert(CoordinatesMode.Drawing, scale);
            d.Convert(CoordinatesMode.Drawing, scale);
            SetSize(new UnifiedPoint(d.X - c.X, d.Y - c.Y, CoordinatesMode.Drawing), scale);
            return;
        }
        if(c > d)
        {
            SetPosition(d, scale);
            c.Convert(CoordinatesMode.Drawing, scale);
            d.Convert(CoordinatesMode.Drawing, scale);
            SetSize(new UnifiedPoint(c.X - d.X, c.Y - d.Y, CoordinatesMode.Drawing), scale);
            return;
        }

        if (c == d)
        {
            SetPosition(c);
            SetSize(new UnifiedPoint(0, 0), scale);
        }
    }
}