using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Input;
using MRCR.datastructures;
using MRCR.Editor;
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
    private const double InitialScale = 20;
    private ToolSetOrganizacja _toolSetOrganizacja;
    
    private Dictionary<EditorMode, ICanvasManager> _canvasManagers;
    private Dictionary<string, ITreeManager> _treeManagers;
    internal readonly CanvasMediator CanvasMediator;
    internal World World { get;}
    public EditorWindow(string worldPath)
    {
        InitializeComponent();
        (Left, Top)  = (0, 0);
        World = World.Load(worldPath);
        ContentToolBar.Content = _toolSetOrganizacja = new ToolSetOrganizacja();
        _canvasManagers = new()
        {
            { EditorMode.Organization, new OrganizationCanvasManager(CanvasOrganizationMap, InitialScale)}
        };
        
        _treeManagers = new()
        {
            { "objectsTrails", new ObjectsTrailsTreeManager() },
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
                .ButtonRelease(new UnifiedPoint(Math.Round(p.X/InitialScale), Math.Round(p.Y/InitialScale)));
        }
        catch (NotImplementedException)
        {
            MessageBox.Show("Nie zaimplementowano tego typu akcji", "Nie zaimplementowano", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void CanvasOrganizationMap_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        PointFloat p = e.GetPosition(CanvasOrganizationMap);
        p.X = Math.Round(p.X/InitialScale);
        p.Y = Math.Round(p.Y/InitialScale);
        State.Text = "LPM Down: " + p;
    }

    private void CanvasOrganizationMap_OnMouseMove(object sender, MouseEventArgs e)
    {
        PointFloat p = e.GetPosition(CanvasOrganizationMap);
        p.X = Math.Round(p.X/InitialScale);
        p.Y = Math.Round(p.Y/InitialScale);
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
        OrganizationCanvasManager? ocm = _canvasManagers[EditorMode.Organization] as OrganizationCanvasManager;
        if (ocm == null) throw new InvalidDataException("CanvasManager is not OrganizationCanvasManager");
        ocm.GenerateGrid(size, InitialScale);
    }
    private void CanvasOrganizationMap_OnSizeChanged(object sender, SizeChangedEventArgs e)
    {
        GenerateOrganizationMapGrid(e.NewSize);
        _canvasManagers[EditorMode.Organization].UpdateCanvas();
    }
}