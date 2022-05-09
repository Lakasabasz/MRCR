using System.Threading;
using System.Windows;
using System.Windows.Controls;
using MRCR.Editor_UC;
using NUnit.Framework;

namespace MRCR_tests;

public class ToolSetOrganizacjaTest
{
    [Test, Apartment(ApartmentState.STA)]
    public void ToolSetOrganizacjaShowTest()
    {
        ToolSetOrganizacja tso = new ToolSetOrganizacja();
        Window w = new Window
        {
            Content = tso
        };
        w.ShowDialog();
    }
}