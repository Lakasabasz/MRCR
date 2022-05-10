using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using MRCR.canvasdrawable;
using MRCR.datastructures;
using MRCR.Editor_UC;

using Ellipse = MRCR.canvasdrawable.Ellipse;
using PointFloat = System.Windows.Point;
using PointInt = System.Drawing.Point;
using SizeInt = System.Drawing.Size;
using SizeFloat = System.Windows.Size;

namespace MRCR;

public partial class Editor : Window
{
    private ToolSetOrganizacja _toolSetOrganizacja;
    private double _scale = 20;
    private bool _lpm = false;
    private Dictionary<string, Tuple<List<IDrawableProxy>, bool>> _canvasShapes;
    internal World World { get;}
    public Editor(string worldPath)
    {
        InitializeComponent();
        Left  = 0;
        Top   = 0;
        World = World.Load(worldPath);
        _toolSetOrganizacja = new ToolSetOrganizacja();
        ContentToolBar.Content = _toolSetOrganizacja;
        CanvasOrganizationMap.UpdateLayout();
        _canvasShapes = new();
    }
    public void CanvasPress(PointInt point)
    {
        throw new System.NotImplementedException();
    }

    public void CanvasRelease(PointInt point)
    {
        // Check current tab
        // Check current tool
        throw new System.NotImplementedException();
    }

    private void CanvasOrganizationMap_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        _lpm = false;
        PointFloat p = e.GetPosition(CanvasOrganizationMap);
        p.X = Math.Round(p.X/_scale);
        p.Y = Math.Round(p.Y/_scale);
        State.Text = "LPM Up: " + p;
    }

    private void CanvasOrganizationMap_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        _lpm = true;
        PointFloat p = e.GetPosition(CanvasOrganizationMap);
        p.X = Math.Round(p.X/_scale);
        p.Y = Math.Round(p.Y/_scale);
        State.Text = "LPM Down: " + p;
    }

    private void CanvasOrganizationMap_OnMouseMove(object sender, MouseEventArgs e)
    {
        PointFloat p = e.GetPosition(CanvasOrganizationMap);
        p.X = Math.Round(p.X/_scale);
        p.Y = Math.Round(p.Y/_scale);
        State.Text = "LPM " + (_lpm ? "Down" : "Up") + " Move: " + p;
    }

    private void CanvasOrganizationMap_OnLoaded(object sender, RoutedEventArgs e)
    {
        var (h, w) = (CanvasOrganizationMap.ActualHeight, CanvasOrganizationMap.ActualWidth);
        GenerateOrganizationMapGrid(new SizeFloat(w, h));
        RenderCanvasOrganizationMap();
    }
    private void GenerateOrganizationMapGrid(SizeFloat size)
    {
        Tuple<List<IDrawableProxy>, bool> gridDots;
        gridDots = _canvasShapes.ContainsKey("gridDots") ? _canvasShapes["gridDots"] : new Tuple<List<IDrawableProxy>, bool>(new(), true);
        
        int spacing = 5;
        for (int i = 0; i < size.Width / _scale; i++)
        {
            for (int j = 0; j < size.Height/_scale; j++)
            {
                if (gridDots.Item1.Any(x => x.IsOnPosition(new PointInt(i, j)))) continue;
                Ellipse dot;
                if(i % spacing == 0 && j % spacing == 0)
                    dot = new Ellipse(new SizeInt(5, 5), new PointInt(i, j), Brushes.Gray, _scale);
                else dot = new Ellipse(new SizeInt(5, 5), new PointInt(i, j), Brushes.LightGray, _scale);
                gridDots.Item1.Add(dot);
            }
        }
        _canvasShapes["gridDots"] = gridDots;
    }
    private void RenderCanvasOrganizationMap()
    {
        CanvasOrganizationMap.Children.Clear();
        foreach (var (_, (drawable, active)) in _canvasShapes)
        {
            if(!active) continue;
            drawable.ForEach(d => CanvasOrganizationMap.Children.Add(d.GetDrawable()));
        }
    }
    private void CanvasOrganizationMap_OnSizeChanged(object sender, SizeChangedEventArgs e)
    {
        GenerateOrganizationMapGrid(e.NewSize);
        RenderCanvasOrganizationMap();
    }
}