using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace MRCR;

public partial class OEDWorld : UserControl
{
    private Window _parentWindow;
    
    public static readonly RoutedEvent OEDCreateWorldEvent = EventManager.RegisterRoutedEvent(
        "OEDCreateWorld", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(OEDWorld));
    
    public static readonly RoutedEvent OEDBackEvent = EventManager.RegisterRoutedEvent(
        "OEDBack", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(OEDWorld));
    
    public event RoutedEventHandler OEDCreateWorld
    {
        add { AddHandler(OEDCreateWorldEvent, value); }
        remove { RemoveHandler(OEDCreateWorldEvent, value); }
    }
    public event RoutedEventHandler OEDBack
    {
        add { AddHandler(OEDBackEvent, value); }
        remove { RemoveHandler(OEDBackEvent, value); }
    }
    void RaiseOEDCreateWorldEvent()
    {
        RoutedEventArgs routedEventArgs = new RoutedEventArgs(OEDCreateWorldEvent);
        RaiseEvent(routedEventArgs);
    }
    void RaiseOEDBackEvent()
    {
        RoutedEventArgs routedEventArgs = new RoutedEventArgs(OEDBackEvent);
        RaiseEvent(routedEventArgs);
    }
    
    public OEDWorld(Window parentWindow)
    {
        InitializeComponent();
        _parentWindow = parentWindow;
        Loaded += ReloadWorldList;
    }
    private void ReloadWorldList(object sender, RoutedEventArgs e)
    {
        ReloadWorldList();
    }
    private void LbWorldsList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if(LbWorldsList.SelectedIndex != -1)
        {
            BtDelete.IsEnabled = true;
            BtEdit.IsEnabled = true;
            BtOpen.IsEnabled = true;
        }
        else
        {
            BtDelete.IsEnabled = false;
            BtEdit.IsEnabled = false;
            BtOpen.IsEnabled = false;
        }
    }

    private void BtOpen_OnClick(object sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void BtNew_OnClick(object sender, RoutedEventArgs e)
    {
        RaiseOEDCreateWorldEvent();
    }

    private void BtEdit_OnClick(object sender, RoutedEventArgs e)
    {
        WorldSchema? ws = LbWorldsList.SelectedItem as WorldSchema;
        if (ws == null) return;
        Editor editorScreen = new Editor(Config.WorldDirectoryPath + ws.Name + Config.WorldFileExtension);
        _parentWindow.Hide();
        editorScreen.ShowDialog();
        _parentWindow.Show();
    }

    private void BtDelete_OnClick(object sender, RoutedEventArgs e)
    {
        WorldSchema? worldName = LbWorldsList.SelectedItem as WorldSchema;
        if (worldName == null) return;
        try
        {
            File.Delete(Config.WorldDirectoryPath + worldName.Name + Config.WorldFileExtension);
            ReloadWorldList();
        }
        catch (IOException)
        {
            MessageBox.Show(
                "Nie można usunąć pliku " + worldName.Name + Config.WorldFileExtension,
                "Błąd usunięcia pliku",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }

    public void ReloadWorldList()
    {
        List<WorldSchema> worldSchemas = new List<WorldSchema>();
        try{
            string[] worlds = Directory.GetFiles(Config.WorldDirectoryPath, "*" + Config.WorldFileExtension);
            foreach (string world in worlds)
            {
                string name = Path.GetFileNameWithoutExtension(world);
                WorldSchema ws = new WorldSchema() { Name = name };
                worldSchemas.Add(ws);
            }
        }
        catch(IOException)
        {
            Console.WriteLine("[WARNING] Worlds folder not found");
            Directory.CreateDirectory(Config.WorldDirectoryPath);
        }

        LbWorldsList.ItemsSource = worldSchemas;
    }

    private void Back_OnClick(object sender, RoutedEventArgs e)
    {
        RaiseOEDBackEvent();
    }
}

public class WorldSchema
{
    public string Name { get; set; }
}