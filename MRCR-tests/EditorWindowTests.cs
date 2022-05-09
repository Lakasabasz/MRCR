using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using MRCR;
using MRCR.datastructures;
using MRCR.datastructures.serializable;
using MRCR.Editor_UC;
using NUnit.Framework;
using Point = System.Drawing.Point;

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

    [Test, Apartment(ApartmentState.STA), NonParallelizable, Order(1)]
    public void EditorWindowBeginStateTest()
    {
        string filename = SetupWorldFile();
        Editor editor = new Editor(filename);
        ToolSetOrganizacja? tso = editor.ContentToolBar.Content as ToolSetOrganizacja;
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
        Assert.IsTrue(editor.TiOrganizacja.IsEnabled);
        Assert.IsFalse(editor.TiSzlak.IsEnabled);
        Assert.IsFalse(editor.TiSchem.IsEnabled);
        Assert.IsFalse(editor.TiFiz.IsEnabled);
        Assert.IsFalse(editor.TiDepend.IsEnabled);
        
        Assert.IsEmpty(editor.TreeSzlakPost.Items);
        Assert.IsEmpty(editor.TreeLines.Items);
        Assert.IsEmpty(editor.TreeLCS.Items);
        
        Assert.IsEmpty(editor.SpSoProperties.Children);
        editor.ShowDialog();
    }

    [Test, Apartment(ApartmentState.STA), NonParallelizable, Order(2)]
    public void EditorWindowButtonTest()
    {
        string filename = SetupWorldFile();
        Editor editor = new Editor(filename);
        ToolSetOrganizacja? tso = editor.ContentToolBar.Content as ToolSetOrganizacja;
        tso.BtAddPost.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
        Assert.IsTrue(tso.BtAddPost.IsChecked);
        tso.BtAddDepot.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
        Assert.IsTrue(tso.BtAddDepot.IsChecked);
        Assert.IsFalse(tso.BtAddPost.IsChecked);
        tso.BtAddStationDepot.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
        Assert.IsTrue(tso.BtAddStationDepot.IsChecked);
        Assert.IsFalse(tso.BtAddDepot.IsChecked);
        tso.BtMove.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
        Assert.IsTrue(tso.BtMove.IsChecked);
        Assert.IsFalse(tso.BtAddStationDepot.IsChecked);
        tso.BtSelect.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
        Assert.IsTrue(tso.BtSelect.IsChecked);
        Assert.IsFalse(tso.BtMove.IsChecked);
        Assert.AreEqual(Visibility.Visible, tso.SWpSelect.Visibility);
        Assert.IsTrue(tso.SBtSelectObject.IsChecked);
        tso.SBtSelectTrail.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
        Assert.IsTrue(tso.SBtSelectTrail.IsChecked);
        Assert.IsFalse(tso.SBtSelectObject.IsChecked);
        Assert.IsTrue(tso.BtSelect.IsChecked);
        tso.SBtSelectLine.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
        Assert.IsTrue(tso.SBtSelectLine.IsChecked);
        Assert.IsFalse(tso.SBtSelectTrail.IsChecked);
        tso.SBtSelectLCS.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
        Assert.IsTrue(tso.SBtSelectLCS.IsChecked);
        Assert.IsFalse(tso.SBtSelectLine.IsChecked);
        tso.BtDelete.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
        Assert.AreEqual(tso.SWpSelect.Visibility, Visibility.Collapsed);
        Assert.IsFalse(tso.BtSelect.IsChecked);
        Assert.IsFalse(tso.SBtSelectLCS.IsChecked);
        Assert.IsTrue(tso.BtDelete.IsChecked);
        Assert.IsTrue(tso.SBtDeleteObject.IsChecked);
        tso.SBtDeleteTrail.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
        Assert.IsTrue(tso.SBtDeleteTrail.IsChecked);
        Assert.IsFalse(tso.SBtDeleteObject.IsChecked);
        Assert.IsTrue(tso.BtDelete.IsChecked);
        tso.SBtDeleteLine.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
        Assert.IsTrue(tso.SBtDeleteLine.IsChecked);
        Assert.IsFalse(tso.SBtDeleteTrail.IsChecked);
        tso.SBtDeleteLCS.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
        Assert.IsTrue(tso.SBtDeleteLCS.IsChecked);
        Assert.IsFalse(tso.SBtDeleteLine.IsChecked);
        tso.BtDelete.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
    }

    [Test, Apartment(ApartmentState.STA), NonParallelizable, Order(3)]
    public void EditorWindowAddPostTest()
    {
        string filename = SetupWorldFile();
        Editor editor = new Editor(filename);
        ToolSetOrganizacja? tso = editor.ContentToolBar.Content as ToolSetOrganizacja;
        tso.BtAddPost.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
        editor.CanvasRelease(new Point(5, 5));
        SerializableGraph sg = JsonSerializer.Deserialize<SerializableGraph>(editor.World.Serialize());
        Assert.IsNotEmpty(sg.vertices);
        Assert.IsNotEmpty(editor.TreeSzlakPost.Items);
        Assert.AreEqual(1, editor.TreeSzlakPost.Items.Count);
        Assert.IsNotNull(sg.subgraphs);

        Assert.IsNotEmpty(editor.TreeLCS.Items);
        TreeViewItem? treeItemLCS = editor.TreeLCS.Items[0] as TreeViewItem;
        Assert.IsNotNull(treeItemLCS);
        Assert.IsTrue(treeItemLCS.IsExpanded);
        Assert.IsNotEmpty(treeItemLCS.Items);
    }
}