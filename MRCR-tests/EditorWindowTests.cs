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
using MRCR.Editor_UC;
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

        World world = new World { Name = $"WorldFile {_lastFileID}" };
        FileStream fs = File.Create(filename);
        UnicodeEncoding unicode = new UnicodeEncoding();
        fs.Write(unicode.GetBytes(JsonSerializer.Serialize(world)), 0,
            unicode.GetByteCount(JsonSerializer.Serialize(world)));
        fs.Close();
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
    public void EditorWindowAddPostTest()
    {
        string filename = SetupWorldFile();
        Editor editor = new Editor(filename);
        ToolSetOrganizacja? tso = editor.ContentToolBar.Content as ToolSetOrganizacja;
        tso.BtAddPost.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
        editor.CanvasPress(new Point(5, 5));
        Assert.IsNotEmpty(editor.World.Posts);
        Assert.IsNotEmpty(editor.TreeSzlakPost.Items);
        Assert.AreEqual(1, editor.TreeSzlakPost.Items.Count);
        Assert.IsNotEmpty(editor.TreeLines.Items);
        TreeViewItem? treeItem = editor.TreeLines.Items[0] as TreeViewItem;
        Assert.IsNotNull(treeItem);
        Assert.IsTrue(treeItem.IsExpanded);
        Assert.IsNotNull(treeItem.Items);
        Assert.IsNotEmpty(editor.TreeLCS.Items);
        TreeViewItem? treeItemLCS = editor.TreeLCS.Items[0] as TreeViewItem;
        Assert.IsNotNull(treeItemLCS);
        Assert.IsTrue(treeItemLCS.IsExpanded);
        Assert.IsNotEmpty(treeItemLCS.Items);
    }
}