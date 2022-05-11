using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using MRCR.canvasdrawable;
using MRCR.datastructures;

namespace MRCR.Editor;

public class OrganizationCanvasManager : ICanvasManager
{
    private Canvas _canvas;
    private Dictionary<string, SwitchableCategory<NameableIDrawableProxy>> _canvasDrawables;
    private double _scale;
    public OrganizationCanvasManager(Canvas canvasOrganizationMap, double scale)
    {
        _canvas = canvasOrganizationMap;
        _canvasDrawables = new();
        _scale = scale;
    }

    public void AddUiElement(IDrawableProxy element, string category, string? name)
    {
        if (!_canvasDrawables.ContainsKey(category))
        {
            _canvasDrawables.Add(category,
                new SwitchableCategory<NameableIDrawableProxy>(new(), true));
        }
        _canvasDrawables[category].Item1.Add(new NameableIDrawableProxy(element, name));
    }

    public void RemoveUiElement(string category, string name)
    {
        throw new System.NotImplementedException();
    }

    public void ClearCategory(string category)
    {
        throw new System.NotImplementedException();
    }

    public void DisableCategory(string category)
    {
        if(_canvasDrawables.ContainsKey(category))
        {
            _canvasDrawables[category].Item2 = false;
        }
    }

    public void EnableCategory(string category)
    {
        throw new System.NotImplementedException();
    }

    public List<NameableIDrawableProxy> GetCategory(string category)
    {
        if(!_canvasDrawables.ContainsKey(category))
        {
            _canvasDrawables.Add(category, new SwitchableCategory<NameableIDrawableProxy>(new(), true));
        }

        return _canvasDrawables[category].Item1;
    }

    public void UpdateCanvas()
    {
        _canvas.Children.Clear();
        foreach (var category in _canvasDrawables.Where(x => x.Value.Item2))
        {
            foreach (var element in category.Value.Item1)
            {
                _canvas.Children.Add(element.Item1.GetDrawable());
            }
        }
        Rectangle rect = new Rectangle();
        rect.Width = _canvas.ActualWidth + 100;
        rect.Height = _canvas.ActualHeight + 100;
        Canvas.SetTop(rect, -50);
        Canvas.SetLeft(rect, -50);
        rect.Fill = Brushes.Transparent;
        _canvas.Children.Add(rect);
    }

    public UnifiedPoint ToDrawingCoordinates(UnifiedPoint point)
    {
        return new UnifiedPoint
        {
            X = point.X * _scale,
            Y = point.Y * _scale
        };
    }
}
