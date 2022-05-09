using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MRCR.Editor_UC;

public partial class ToolSetOrganizacja : UserControl
{
    public ToolSetOrganizacja()
    {
        InitializeComponent();
    }

    private void BtAddPost_OnToolTipClosing(object sender, ToolTipEventArgs e)
    {
        e.Handled = true;
    }
}