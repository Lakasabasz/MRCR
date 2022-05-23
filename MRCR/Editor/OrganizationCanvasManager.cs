using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MRCR.canvasdrawable;
using MRCR.datastructures;
using Ellipse = MRCR.canvasdrawable.Ellipse;
using PointFloat = System.Windows.Point;
using PointInt = System.Drawing.Point;
using Rectangle = MRCR.canvasdrawable.Rectangle;
using SizeInt = System.Drawing.Size;
using SizeFloat = System.Windows.Size;

namespace MRCR.Editor;

public class OrganizationCanvasManager : ICanvasManager
{
    private Canvas _canvas;
    private Dictionary<string, SwitchableCategory<NameableIDrawableProxy>> _canvasDrawables;
    private double _scale;
    public OrganizationCanvasManager(Canvas canvasOrganizationMap, double scale, World world)
    {
        _canvas = canvasOrganizationMap;
        _canvasDrawables = new();
        _scale = scale;
        world.RegisterDelegate(OrganisationObjectType.Post, OnObjectChanged);
        world.RegisterDelegate(OrganisationObjectType.Trail, OnObjectChanged);
        world.RegisterDelegate(OrganisationObjectType.Line, OnObjectChanged);
        world.RegisterDelegate(OrganisationObjectType.Control, OnObjectChanged);
        world.OnWorldStateChanged += OnObjectAdded;
    }

    public double Scale { get=>_scale; set=>_scale=value; }

    public void AddUiElement(IDrawableProxy element, string category, string? name)
    {
        if (!_canvasDrawables.ContainsKey(category))
        {
            _canvasDrawables.Add(category,
                new SwitchableCategory<NameableIDrawableProxy>(new(), true));
        }

        element.UpdateScale(_scale);
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
            _canvasDrawables[category].Item2 = false;
    }

    public void EnableCategory(string category)
    {
        if(_canvasDrawables.ContainsKey(category))
            _canvasDrawables[category].Item2 = true;
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
        System.Windows.Shapes.Rectangle rect = new System.Windows.Shapes.Rectangle();
        rect.Width = _canvas.ActualWidth + 100;
        rect.Height = _canvas.ActualHeight + 100;
        Canvas.SetTop(rect, -50);
        Canvas.SetLeft(rect, -50);
        Canvas.SetZIndex(rect, 2);
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

    public UnifiedPoint ToUniform(MouseButtonEventArgs args)
    {
        var p = args.GetPosition(_canvas);
        return new UnifiedPoint(Math.Round(p.X / _scale), Math.Round(p.Y / _scale));
    }

    public List<NameableIDrawableProxy> GetOverlappingObjects(IDrawableProxy positionedObject, string? category = null)
    {
        var points = positionedObject.GetBorderPoints();
        List<NameableIDrawableProxy> ret = new();
        foreach (var (key, value) in _canvasDrawables)
        {
            if (category != null && key != category) continue;
            foreach (var nameableIDrawableProxy in value.Item1)
            {
                if (points.Any(p => nameableIDrawableProxy.Item1.IsInside(p)))
                {
                    ret.Add(nameableIDrawableProxy);
                }
            }
        }

        return ret;
    }


    public void GenerateGrid(Size size, double scale)
    {
        var gridDots = GetCategory("gridDots");
        int spacing = 5;
        for (int i = 0; i < size.Width / scale; i++)
        {
            for (int j = 0; j < size.Height/scale; j++)
            {
                if (gridDots.Any(x => x.Item1.IsOnPosition(new PointInt(i, j)))) continue;
                Brush brush = Brushes.LightGray;
                if (i % spacing == 0 && j % spacing == 0) brush = Brushes.Gray;
                Ellipse dot = new Ellipse(new SizeInt(2, 2), new PointInt(i, j), brush, scale);
                gridDots.Add(new NameableIDrawableProxy(dot, null));
            }
        }
    }

    public void OnObjectAdded(object? sender, IOrganizationStructure organizationStructure)
    {
        if (organizationStructure.Type != OrganisationObjectType.Post) return;
        var post = organizationStructure as Post;
        if(post == null) throw new ArgumentException("organizationStructure is not a post but type is Post");
        Brush br = Brushes.Blue;
        switch (post.PostType)
        {
            case PostType.Post:
                br = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/MRCR;component/icons/add-train-post.png")));
                break;
            case PostType.Depot :
                br = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/MRCR;component/icons/add-train-depot.png")));
                break;
            case PostType.Combined:
                br = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/MRCR;component/icons/add-train-station.png")));
                break;
        }
        UnifiedPoint mouseCoords = new UnifiedPoint(post.GetPosition().X, post.GetPosition().Y, CoordinatesMode.World);
        mouseCoords.Convert(CoordinatesMode.Drawing, Scale);
        AddUiElement(
            new Rectangle(
                new UnifiedPoint(30, 30, CoordinatesMode.Drawing),
                mouseCoords.Move(-15, -15), br,
                Scale),
            "objects", post.GetName());
        UpdateCanvas();
    }
    
    public void OnObjectChanged(object? sender, EventArgs eventArgs)
    {
        if (sender is Post p)
        {
            foreach (var (item1, item2) in _canvasDrawables["objects"].Item1)
            {
                if (item2 == p.GetName()) item1.IsSelected = p.IsSelected;
            }
        }
    }
}
