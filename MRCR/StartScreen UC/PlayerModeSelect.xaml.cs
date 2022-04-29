using System.Windows;
using System.Windows.Controls;

namespace MRCR.StartScreen_UC;

public partial class PlayerModeSelect : UserControl
{
    public static readonly RoutedEvent SingleplayerChooseEvent = EventManager.RegisterRoutedEvent(
        "SingleplayerChoose", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(StartScreen));
        
    public static readonly RoutedEvent MultiplayerChooseEvent = EventManager.RegisterRoutedEvent(
        "MultiplayerChoose", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(StartScreen));
    public event RoutedEventHandler SingleplayerChoose
    {
        add { AddHandler(SingleplayerChooseEvent, value); }
        remove { RemoveHandler(SingleplayerChooseEvent, value); }
    }
    public event RoutedEventHandler MultiplayerChoose
    {
        add { AddHandler(MultiplayerChooseEvent, value); }
        remove { RemoveHandler(MultiplayerChooseEvent, value); }
    }
    void RaiseSingleplayerChooseEvent()
    {
        RoutedEventArgs newEventArgs = new RoutedEventArgs(SingleplayerChooseEvent);
        RaiseEvent(newEventArgs);
    }
    void RaiseMultiplayerChooseEvent()
    {
        RoutedEventArgs newEventArgs = new RoutedEventArgs(MultiplayerChooseEvent);
        RaiseEvent(newEventArgs);
    }
    
    public PlayerModeSelect()
    {
        InitializeComponent();
    }

    private void Singleplayer_OnClick(object sender, RoutedEventArgs e)
    {
        RaiseSingleplayerChooseEvent();
    }
    
    private void Multiplayer_OnClick(object sender, RoutedEventArgs e)
    {
        RaiseMultiplayerChooseEvent();
    }

    private void Close_OnClick(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }
}