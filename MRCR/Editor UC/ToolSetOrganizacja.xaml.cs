using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MRCR.Editor_UC;

public enum ActionType
{
    CREATE_POST,
    CREATE_DEPOT,
    CREATE_STATION,
    MOVE,
    SELECT_OBJECT, SELECT_TRAIL, SELECT_LINE, SELECT_LCS,
    DELETE_OBJECT, DELETE_TRAIL, DELETE_LINE, DELETE_LCS,
    CONNECT,
    MERGE,
    EXCLUDE
}

public partial class ToolSetOrganizacja : UserControl
{
    private static readonly Dictionary<string, ActionType> mapActionTypes = new()
    {
        { "BtAddPost", ActionType.CREATE_POST },
        { "BtAddDepot", ActionType.CREATE_DEPOT },
        { "BtAddStationDepot", ActionType.CREATE_STATION },
        { "BtMove", ActionType.MOVE },
        { "SBtSelectObject", ActionType.SELECT_OBJECT },
        { "SBtSelectTrail", ActionType.SELECT_TRAIL },
        { "SBtSelectLine", ActionType.SELECT_LINE },
        { "SBtSelectLCS", ActionType.SELECT_LCS },
        { "SBtDeleteObject", ActionType.DELETE_OBJECT },
        { "SBtDeleteTrail", ActionType.DELETE_TRAIL },
        { "SBtDeleteLine", ActionType.DELETE_LINE },
        { "SBtDeleteLCS", ActionType.DELETE_LCS },
        { "BtConnect", ActionType.CONNECT },
        { "BtMerge", ActionType.MERGE },
        { "BtExclude", ActionType.EXCLUDE }
    };
    public ActionType CurrentActionType { get; set; }
    public ToolSetOrganizacja()
    {
        InitializeComponent();
    }
    private ActionType GetCurrentMode()
    {
        foreach (var button in MainTools.Children)
        {
            RadioButton? rb = button as RadioButton;
            if(rb == null) continue;
            if(rb.IsChecked == false) continue;
            if(mapActionTypes.ContainsKey(rb.Name)) return mapActionTypes[rb.Name];
            if (rb.Name.Equals("BtSelect"))
            {
                foreach (var subbutton in SWpSelect.Children)
                {
                    RadioButton subrb = (subbutton as RadioButton)!;
                    if(subrb.IsChecked == false) continue;
                    return mapActionTypes[subrb.Name];
                }
            }
            if (rb.Name.Equals("BtDelete"))
            {
                foreach (var subbuton in SWpDelete.Children)
                {
                    RadioButton subrb = (subbuton as RadioButton)!;
                    if(subrb.IsChecked == false) continue;
                    return mapActionTypes[subrb.Name];
                }
            }
        }
        throw new Exception("Undefined state");
    }

    private void BtSelect_OnChecked(object sender, RoutedEventArgs e) => SWpSelect.Visibility = Visibility.Visible;
    private void BtSelect_OnUnchecked(object sender, RoutedEventArgs e) => SWpSelect.Visibility = Visibility.Collapsed;
    private void BtDelete_OnChecked(object sender, RoutedEventArgs e) => SWpDelete.Visibility = Visibility.Visible;
    private void BtDelete_OnUnchecked(object sender, RoutedEventArgs e) => SWpDelete.Visibility = Visibility.Collapsed;
    private void ButtonBase_OnClick(object sender, RoutedEventArgs e) => CurrentActionType = GetCurrentMode();
}