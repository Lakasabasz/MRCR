using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using MRCR.datastructures;

namespace MRCR;

public partial class CreateNewWorld : UserControl
{
    private Window _parrent;
    private static readonly RoutedEvent CancelWorldCreation = EventManager.RegisterRoutedEvent(
        "CancelWorldCreation", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(CreateNewWorld));
    
    public event RoutedEventHandler CancelWorldCreationEvent
    {
        add { AddHandler(CancelWorldCreation, value); }
        remove { RemoveHandler(CancelWorldCreation, value); }
    }
    
    private void RaiseCancelWorldCreationEvent()
    {
        RoutedEventArgs newEventArgs = new RoutedEventArgs(CreateNewWorld.CancelWorldCreation);
        RaiseEvent(newEventArgs);
    }
    
    public CreateNewWorld(Window parrent)
    {
        InitializeComponent();
        _parrent = parrent;
    }

    private void WorldName_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        CreateButton.IsEnabled = WorldName.Text.Length != 0;
    }

    private void CancelButton_OnClick(object sender, RoutedEventArgs e)
    {
        RaiseCancelWorldCreationEvent();
    }

    private void CreateButton_OnClick(object sender, RoutedEventArgs e)
    {
        try
        {
            FileStream world = File.Create(Config.WorldDirectoryPath + WorldName.Text + Config.WorldFileExtension);
            World newWorld = new World{name = WorldName.Text};;
            string json = JsonSerializer.Serialize(newWorld);
            UnicodeEncoding unicode = new UnicodeEncoding();
            world.Write(unicode.GetBytes(json), 0, unicode.GetByteCount(json));
            _parrent.Hide();
            Editor editor = new Editor(Config.WorldDirectoryPath + WorldName.Text + Config.WorldFileExtension);
            editor.ShowDialog();
            _parrent.Show();
            RaiseCancelWorldCreationEvent();
        }
        catch (ArgumentException)
        {
            MessageBox.Show(
                "Utworzenie świata jest nie możliwe. Niepoprawna nazwa świata.",
                "Błąd tworzenia świata",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
        catch (PathTooLongException)
        {
            MessageBox.Show(
                "Utworzenie świata jest nie możliwe. Nazwa świata jest za długa.",
                "Błąd tworzenia świata",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
        catch (IOException)
        {
            MessageBox.Show(
                "Utworzenie świata jest nie możliwe. Istnieje już plik o takiej nazwie, w nazwie zostały wykorzystane niedozwolone znaki lub wystąpił inny błąd.",
                "Błąd tworzenia świata",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }
}