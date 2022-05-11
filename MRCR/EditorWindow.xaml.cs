using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using MRCR.datastructures;
using MRCR.Editor;
using Ellipse = MRCR.canvasdrawable.Ellipse;
using PointFloat = System.Windows.Point;
using PointInt = System.Drawing.Point;
using SizeInt = System.Drawing.Size;
using SizeFloat = System.Windows.Size;

namespace MRCR;

public enum EditorMode
{
    Organization,
    Trails,
    PostSchema,
    PhysicalSchema,
    Dependencies,
    TimeTable
};

public partial class EditorWindow : Window
{
    private ToolSetOrganizacja _toolSetOrganizacja;
    private double _scale = 20;
    private Dictionary<EditorMode, ICanvasManager> _canvasManagers;
    internal readonly CanvasMediator CanvasMediator;
    internal World World { get;}
    public EditorWindow(string worldPath)
    {
        InitializeComponent();
        Left  = 0;
        Top   = 0;
        World = World.Load(worldPath);
        _toolSetOrganizacja = new ToolSetOrganizacja();
        ContentToolBar.Content = _toolSetOrganizacja;
        _canvasManagers = new()
        {
            { EditorMode.Organization, new OrganizationCanvasManager(CanvasOrganizationMap, _scale)}
        };
        CanvasMediator = new CanvasMediator();
        CanvasMediator.Register(
            EditorMode.Organization, ActionType.CREATE_POST, new OrganizationCreatePostMediator(World, _canvasManagers[EditorMode.Organization]));
        CanvasMediator.Register(
            EditorMode.Organization, ActionType.CREATE_DEPOT, new OrganizationCreateDepotMediator(World, _canvasManagers[EditorMode.Organization]));
        CanvasMediator.Register(
            EditorMode.Organization, ActionType.CREATE_STATION, new OrganizationCreateStationMediator(World, _canvasManagers[EditorMode.Organization]));
    }

    private void CanvasOrganizationMap_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        PointFloat p = e.GetPosition(CanvasOrganizationMap);
        State.Text = "LPM Up: " + p;
        try
        {
            CanvasMediator.Mediate(EditorMode.Organization, _toolSetOrganizacja.CurrentActionType)
                .ButtonRelease(new UnifiedPoint(Math.Round(p.X/_scale), Math.Round(p.Y/_scale)));
        }
        catch (NotImplementedException)
        {
            MessageBox.Show("Nie zaimplementowano tego typu akcji", "Nie zaimplementowano", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void CanvasOrganizationMap_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
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
        State.Text = "LPM " + " Move: " + p;
    }

    private void CanvasOrganizationMap_OnLoaded(object sender, RoutedEventArgs e)
    {
        var (h, w) = (CanvasOrganizationMap.ActualHeight, CanvasOrganizationMap.ActualWidth);
        GenerateOrganizationMapGrid(new SizeFloat(w, h));
        _canvasManagers[EditorMode.Organization].UpdateCanvas();
    }
    private void GenerateOrganizationMapGrid(SizeFloat size)
    {
        List<NameableIDrawableProxy> gridDots;
        gridDots = _canvasManagers[EditorMode.Organization].GetCategory("gridDots");
        
        int spacing = 5;
        for (int i = 0; i < size.Width / _scale; i++)
        {
            for (int j = 0; j < size.Height/_scale; j++)
            {
                if (gridDots.Any(x => x.Item1.IsOnPosition(new PointInt(i, j)))) continue;
                Ellipse dot;
                if(i % spacing == 0 && j % spacing == 0)
                    dot = new Ellipse(new SizeInt(2, 2), new PointInt(i, j), Brushes.Gray, _scale);
                else dot = new Ellipse(new SizeInt(2, 2), new PointInt(i, j), Brushes.LightGray, _scale);
                gridDots.Add(new NameableIDrawableProxy(dot, null));
            }
        }
    }
    private void CanvasOrganizationMap_OnSizeChanged(object sender, SizeChangedEventArgs e)
    {
        GenerateOrganizationMapGrid(e.NewSize);
        _canvasManagers[EditorMode.Organization].UpdateCanvas();
    }
}