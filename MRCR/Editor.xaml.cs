using System.Windows;
using System.Windows.Controls;
using MRCR.datastructures;
using MRCR.Editor_UC;

namespace MRCR;

public partial class Editor : Window
{
    private ToolSetOrganizacja _toolSetOrganizacja;
    internal World World { get; set; }
    public Editor(string worldPath)
    {
        InitializeComponent();
        World = World.Load(worldPath);
        _toolSetOrganizacja = new ToolSetOrganizacja();
        ContentToolBar.Content = _toolSetOrganizacja;
    }
    public void CanvasPress(Point point)
    {
        throw new System.NotImplementedException();
    }
}