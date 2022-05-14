using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using MRCR;
using MRCR.datastructures;
using MRCR.datastructures.serializable;
using MRCR.Editor;
using NUnit.Framework;

namespace MRCR_tests;

public class EditorWindowTests
{
    private static int _lastFileID = 0;

    [ExcludeFromCodeCoverage]
    private string SetupWorldFile()
    {
        string filename = $"{Config.WorldDirectoryPath}WorldFile {_lastFileID++}{Config.WorldFileExtension}";
        if (!Directory.Exists(Config.WorldDirectoryPath))
        {
            Directory.CreateDirectory(Config.WorldDirectoryPath);
        }

        World world = new World($"WorldFile {_lastFileID}");
        world.Save(filename);
        return filename;
    }

    [Test, Apartment(ApartmentState.STA), NonParallelizable, Explicit]
    public void EditorWindowShowTest()
    {
        string filename = SetupWorldFile();
        EditorWindow editorWindow = new EditorWindow(filename);
        editorWindow.ShowDialog();
    }

    [Test, Apartment(ApartmentState.STA), NonParallelizable, Order(1)]
    public void EditorWindowBeginStateTest()
    {
        string filename = SetupWorldFile();
        EditorWindow editorWindow = new EditorWindow(filename);
        ToolSetOrganizacja? tso = editorWindow.ContentToolBar.Content as ToolSetOrganizacja;
        Assert.IsNotNull(tso);
        Image?[] images =
        {
            tso.BtAddPost.Content as Image,
            tso.BtAddDepot.Content as Image,
            tso.BtAddStationDepot.Content as Image,
            tso.BtAddLine.Content as Image,
            tso.BtAddLCS.Content as Image,
            tso.BtMove.Content as Image,
            tso.BtDelete.Content as Image,
            tso.BtSelect.Content as Image,
            tso.BtMerge.Content as Image,
            tso.BtConnect.Content as Image,
            tso.BtExclude.Content as Image,
            tso.SBtSelectObject.Content as Image,
            tso.SBtSelectTrail.Content as Image,
            tso.SBtSelectLine.Content as Image,
            tso.SBtSelectLCS.Content as Image,
            tso.SBtDeleteObject.Content as Image,
            tso.SBtDeleteTrail.Content as Image,
            tso.SBtDeleteLine.Content as Image,
            tso.SBtDeleteLCS.Content as Image
        };
        foreach (Image? image in images)
        {
            Assert.IsNotNull(image);
            Assert.IsNotNull(image.Source);
        }
        Assert.IsTrue(editorWindow.TiOrganizacja.IsEnabled);
        Assert.IsFalse(editorWindow.TiSzlak.IsEnabled);
        Assert.IsFalse(editorWindow.TiSchem.IsEnabled);
        Assert.IsFalse(editorWindow.TiFiz.IsEnabled);
        Assert.IsFalse(editorWindow.TiDepend.IsEnabled);
        
        Assert.IsEmpty(editorWindow.TreeSzlakPost.Items);
        Assert.IsEmpty(editorWindow.TreeLines.Items);
        Assert.IsEmpty(editorWindow.TreeLCS.Items);
        
        Assert.IsEmpty(editorWindow.SpSoProperties.Children);
        editorWindow.Show();
    }

    [Test, Apartment(ApartmentState.STA), NonParallelizable, Order(3)]
    public void EditorWindowAddPostTest()
    {
        string filename = SetupWorldFile();
        EditorWindow editorWindow = new EditorWindow(filename);
        ToolSetOrganizacja? tso = editorWindow.ContentToolBar.Content as ToolSetOrganizacja;
        tso.BtAddPost.IsChecked = true;
        tso.BtAddPost.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
        editorWindow.CanvasMediator.Mediate(EditorMode.Organization, tso.CurrentActionType)
            .ButtonRelease(new UnifiedPoint(5, 5));
        
        SerializableGraph sg = JsonSerializer.Deserialize<SerializableGraph>(editorWindow.World.Serialize());
        Assert.IsNotEmpty(sg.vertices);
        Assert.AreEqual(5, sg.vertices[0].X);
        Assert.AreEqual(5, sg.vertices[0].Y);
        Assert.IsNotEmpty(editorWindow.TreeSzlakPost.Items);
        Assert.AreEqual(1, editorWindow.TreeSzlakPost.Items.Count);
        Assert.IsNotNull(sg.subgraphs);

        Assert.IsNotEmpty(editorWindow.TreeLCS.Items);
        TreeViewItem? treeItemControlRoom = editorWindow.TreeLCS.Items[0] as TreeViewItem;
        Assert.IsNotNull(treeItemControlRoom);
        Assert.IsEmpty(treeItemControlRoom.Items);
        Assert.AreEqual("Posterunek 1", treeItemControlRoom.Header);
    }
    
    [Test, Apartment(ApartmentState.STA), NonParallelizable, Order(4)]
    public void EditorWindowAddDepotTest()
    {
        string filename = SetupWorldFile();
        EditorWindow editorWindow = new EditorWindow(filename);
        ToolSetOrganizacja? tso = editorWindow.ContentToolBar.Content as ToolSetOrganizacja;
        tso.BtAddDepot.IsChecked = true;
        tso.BtAddDepot.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
        editorWindow.CanvasMediator.Mediate(EditorMode.Organization, tso.CurrentActionType)
            .ButtonRelease(new UnifiedPoint(5, 5));
        
        Assert.IsNotEmpty(editorWindow.TreeSzlakPost.Items);
        Assert.AreEqual(1, editorWindow.TreeSzlakPost.Items.Count);

        Assert.IsNotEmpty(editorWindow.TreeLCS.Items);
        TreeViewItem? treeItemControlRoom = editorWindow.TreeLCS.Items[0] as TreeViewItem;
        Assert.IsNotNull(treeItemControlRoom);
        Assert.IsEmpty(treeItemControlRoom.Items);
        Assert.AreEqual("Lokomotywownia 1", treeItemControlRoom.Header);
    }
    
}