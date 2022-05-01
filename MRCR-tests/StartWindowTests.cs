using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using Moq;
using MRCR;
using MRCR.datastructures;
using MRCR.services;
using MRCR.StartScreen_UC;
using NUnit.Framework;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace MRCR_tests;

public class StartWindowTests
{
    [SetUp]
    public void Setup()
    {
    }
    
    [TearDown, ExcludeFromCodeCoverage]
    public void CleanUp()
    {
        try
        {
            Directory.Delete(Config.WorldDirectoryPath, true);
        } catch (DirectoryNotFoundException) {}
    }

    [Test, Apartment(ApartmentState.STA), NonParallelizable, Order(1)]
    public void StartWidowsInitTest()
    {
        StartScreen ss = new();
        PlayerModeSelect? c = (PlayerModeSelect) ss.WindowContent.Content;
        Assert.IsNotNull(c);
    }
    
    [Test, Apartment(ApartmentState.STA), NonParallelizable, Order(2)]
    public void StartWindowSingleplayerTest()
    {
        StartScreen ss = new();
        PlayerModeSelect? c = (PlayerModeSelect) ss.WindowContent.Content;
        c.SingleplayerButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        OEDWorld? oedWorld = (OEDWorld) ss.WindowContent.Content;
        Assert.IsNotNull(oedWorld);
    }

    [Test, Apartment(ApartmentState.STA), NonParallelizable, Order(3)]
    public void StartWindowOEDTest1()
    {
        StartScreen ss = new();
        PlayerModeSelect? c = (PlayerModeSelect) ss.WindowContent.Content;
        c.SingleplayerButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        OEDWorld? oedWorld = (OEDWorld) ss.WindowContent.Content;
        Assert.IsFalse(oedWorld.BtOpen.IsEnabled);
        Assert.IsFalse(oedWorld.BtEdit.IsEnabled);
        Assert.IsFalse(oedWorld.BtDelete.IsEnabled);
        Assert.IsEmpty(oedWorld.LbWorldsList.Items);
        Assert.IsTrue(Directory.Exists(Config.WorldDirectoryPath));
        oedWorld.BtNew.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        CreateNewWorld? createNewWorld = (CreateNewWorld) ss.WindowContent.Content;
        Assert.IsNotNull(createNewWorld);
    }

    [Test, Apartment(ApartmentState.STA), NonParallelizable, Order(4)]
    public void StartWindowOEDTest2()
    {
        StartScreen ss = new();
        PlayerModeSelect? c = (PlayerModeSelect) ss.WindowContent.Content;
        c.SingleplayerButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        OEDWorld? oedWorld = (OEDWorld) ss.WindowContent.Content;
        oedWorld.BtBack.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        PlayerModeSelect? c2 = (PlayerModeSelect) ss.WindowContent.Content;
        Assert.IsNotNull(c2);
    }

    [Test, Apartment(ApartmentState.STA), NonParallelizable, Order(5)]
    public void StartWindowCreateWorld1()
    {
        StartScreen ss = new();
        CreateNewWorld.MessageBox = new Mock<IMessageBoxService>().Object;
        CreateNewWorld.FactoryWindow = new Mock<IFactoryWindow>().Object;
        PlayerModeSelect? c = (PlayerModeSelect) ss.WindowContent.Content;
        c.SingleplayerButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        OEDWorld? oedWorld = (OEDWorld) ss.WindowContent.Content;
        oedWorld.BtNew.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        CreateNewWorld? createNewWorld = (CreateNewWorld) ss.WindowContent.Content;
        Assert.IsEmpty(createNewWorld.WorldName.Text);
        Assert.IsFalse(createNewWorld.CreateButton.IsEnabled);
        createNewWorld.WorldName.Text = "TestWorld";
        Assert.IsTrue(createNewWorld.CreateButton.IsEnabled);
        createNewWorld.CreateButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        OEDWorld? oedWorld2 = (OEDWorld) ss.WindowContent.Content;
        Assert.IsNotNull(oedWorld2);
        Assert.IsTrue(File.Exists(Config.WorldDirectoryPath + "TestWorld" + Config.WorldFileExtension));
    }
    
    [Test, Apartment(ApartmentState.STA), NonParallelizable, Order(6)]
    public void StartWindowCreateWorld2()
    {
        StartScreen ss = new();
        PlayerModeSelect? c = (PlayerModeSelect) ss.WindowContent.Content;
        c.SingleplayerButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        OEDWorld? oedWorld = (OEDWorld) ss.WindowContent.Content;
        oedWorld.BtNew.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        CreateNewWorld? createNewWorld = (CreateNewWorld) ss.WindowContent.Content;
        createNewWorld.CancelButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        OEDWorld? oedWorld2 = (OEDWorld) ss.WindowContent.Content;
        Assert.IsNotNull(oedWorld2);
        Assert.IsEmpty(oedWorld2.LbWorldsList.Items);
    }
    
    [Test, Apartment(ApartmentState.STA), NonParallelizable, Order(7)]
    public void StartWindowOEDTest3()
    {
        StartScreen ss = new();
        PlayerModeSelect? c = (PlayerModeSelect) ss.WindowContent.Content;
        c.SingleplayerButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        OEDWorld? oedWorld = (OEDWorld) ss.WindowContent.Content;
        FileStream fs = File.Create(Config.WorldDirectoryPath + "TestWorld" + Config.WorldFileExtension);
        World w = new World { Name = "TestWorld" };
        UnicodeEncoding unicode = new UnicodeEncoding();
        fs.Write(unicode.GetBytes(JsonSerializer.Serialize(w)), 0, unicode.GetByteCount(JsonSerializer.Serialize(w)));
        fs.Close();
        oedWorld.ReloadWorldList();
        Assert.IsNotEmpty(oedWorld.LbWorldsList.Items);
    }

    [Test, Apartment(ApartmentState.STA), NonParallelizable, Order(8)]
    public void StartWindowOEDTest4()
    {
        OEDWorld.FactoryWindow = new Mock<IFactoryWindow>().Object;
        StartScreen ss = new();
        PlayerModeSelect? c = (PlayerModeSelect) ss.WindowContent.Content;
        c.SingleplayerButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        OEDWorld? oedWorld = (OEDWorld) ss.WindowContent.Content;
        World w = new() {Name="TestWorld"};
        UnicodeEncoding unicode = new UnicodeEncoding();
        FileStream fs = File.Create(Config.WorldDirectoryPath + "TestWorld" + Config.WorldFileExtension);
        fs.Write(unicode.GetBytes(JsonSerializer.Serialize(w)), 0, unicode.GetByteCount(JsonSerializer.Serialize(w)));
        fs.Close();
        oedWorld.ReloadWorldList();
        oedWorld.LbWorldsList.SelectedIndex = 0;
        Assert.IsTrue(oedWorld.BtOpen.IsEnabled);
        Assert.IsTrue(oedWorld.BtEdit.IsEnabled);
        Assert.IsTrue(oedWorld.BtDelete.IsEnabled);
        oedWorld.BtEdit.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        OEDWorld? oedWorld2 = (OEDWorld) ss.WindowContent.Content;
        Assert.IsNotNull(oedWorld2);
    }

    [Test, Apartment(ApartmentState.STA), NonParallelizable, Order(5)]
    public void StartWindowOEDTest5()
    {
        OEDWorld.FactoryWindow = new Mock<IFactoryWindow>().Object;
        StartScreen ss = new();
        PlayerModeSelect? c = (PlayerModeSelect) ss.WindowContent.Content;
        c.SingleplayerButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        OEDWorld? oedWorld = (OEDWorld) ss.WindowContent.Content;
        World w = new() {Name="TestWorld"};
        UnicodeEncoding unicode = new UnicodeEncoding();
        FileStream fs = File.Create(Config.WorldDirectoryPath + "TestWorld" + Config.WorldFileExtension);
        fs.Write(unicode.GetBytes(JsonSerializer.Serialize(w)), 0, unicode.GetByteCount(JsonSerializer.Serialize(w)));
        fs.Close();
        oedWorld.ReloadWorldList();
        oedWorld.LbWorldsList.SelectedIndex = 0;
        oedWorld.BtDelete.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        Assert.IsEmpty(oedWorld.LbWorldsList.Items);
    }
}