using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using MRCR.Editor;
using NUnit.Framework;

namespace MRCR_tests;

public class ToolSetOrganizacjaTest
{
    private Window _window;
    [SetUp]
    public void Setup()
    {
        _window = new Window();
        _window.Content = new ToolSetOrganizacja();
    }
    
    [Test, Apartment(ApartmentState.STA), Explicit]
    public void ToolSetOrganizacjaShowTest()
    {
        _window.ShowDialog();
    }
    
    [Test, Apartment(ApartmentState.STA)]
    public void ButtonSingularStabilityTest()
    {
        ToolSetOrganizacja? tso = _window.Content as ToolSetOrganizacja;
        tso.BtAddPost.IsChecked = true;
        tso.BtAddDepot.IsChecked = true;
        Assert.IsTrue(tso.BtAddDepot.IsChecked);
        Assert.IsFalse(tso.BtAddPost.IsChecked);
        tso.BtAddStationDepot.IsChecked = true;
        Assert.IsTrue(tso.BtAddStationDepot.IsChecked);
        Assert.IsFalse(tso.BtAddDepot.IsChecked);
        tso.BtMove.IsChecked = true;
        Assert.IsTrue(tso.BtMove.IsChecked);
        Assert.IsFalse(tso.BtAddStationDepot.IsChecked);
        tso.BtSelect.IsChecked = true;
        Assert.IsTrue(tso.BtSelect.IsChecked);
        Assert.IsFalse(tso.BtMove.IsChecked);
        Assert.AreEqual(Visibility.Visible, tso.SWpSelect.Visibility);
        Assert.IsTrue(tso.SBtSelectObject.IsChecked);
        tso.SBtSelectTrail.IsChecked = true;
        Assert.IsTrue(tso.SBtSelectTrail.IsChecked);
        Assert.IsFalse(tso.SBtSelectObject.IsChecked);
        Assert.IsTrue(tso.BtSelect.IsChecked);
        tso.SBtSelectLine.IsChecked = true;
        Assert.IsTrue(tso.SBtSelectLine.IsChecked);
        Assert.IsFalse(tso.SBtSelectTrail.IsChecked);
        tso.SBtSelectLCS.IsChecked = true;
        Assert.IsTrue(tso.SBtSelectLCS.IsChecked);
        Assert.IsFalse(tso.SBtSelectLine.IsChecked);
        tso.BtDelete.IsChecked = true;
        Assert.AreEqual(tso.SWpSelect.Visibility, Visibility.Collapsed);
        Assert.IsFalse(tso.BtSelect.IsChecked);
        Assert.IsTrue(tso.BtDelete.IsChecked);
        Assert.IsTrue(tso.SBtDeleteObject.IsChecked);
        tso.SBtDeleteTrail.IsChecked = true;
        Assert.IsTrue(tso.SBtDeleteTrail.IsChecked);
        Assert.IsFalse(tso.SBtDeleteObject.IsChecked);
        Assert.IsTrue(tso.BtDelete.IsChecked);
        tso.SBtDeleteLine.IsChecked = true;
        Assert.IsTrue(tso.SBtDeleteLine.IsChecked);
        Assert.IsFalse(tso.SBtDeleteTrail.IsChecked);
        tso.SBtDeleteLCS.IsChecked = true;
        Assert.IsTrue(tso.SBtDeleteLCS.IsChecked);
        Assert.IsFalse(tso.SBtDeleteLine.IsChecked);
    }
}