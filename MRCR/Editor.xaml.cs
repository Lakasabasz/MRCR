using System.Windows;

namespace MRCR;

public partial class Editor : Window
{
    public Editor(string worldPath)
    {
        InitializeComponent();
        WorldPath.Content = worldPath;
    }
}