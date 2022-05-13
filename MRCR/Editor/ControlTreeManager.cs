using System;
using System.Collections.Generic;
using System.Windows.Controls;
using Accessibility;
using MRCR.datastructures;

namespace MRCR.Editor;

class CtmPostBranch
{
    public Post post { get; set; }
    public TreeViewItem tvi { get; set; }
}

class ControllPlaceBranch
{
    public List<CtmPostBranch>? ctmPostBranches { get; set; }
    public TreeViewItem tvi { get; set; }
}

public class ControlTreeManager : ITreeManager
{
    private Dictionary<IControlPlace, ControllPlaceBranch> _drawableControlPlaces;
    private TreeView _treeView;
    private World _world;
    
    public ControlTreeManager(World world, TreeView treeView)
    {
        _treeView = treeView;
        _world = world;
        _drawableControlPlaces = new Dictionary<IControlPlace, ControllPlaceBranch>();
        _world.OnWorldStateChanged += OnWorldStateChanged;
        _world.RegisterDelegate(OrganisationObjectType.Control, OnControlPlaceChanged);
    }

    public void OnWorldStateChanged(object? sender, IOrganizationStructure os)
    {
        if(os is ControlRoom cr){
            _drawableControlPlaces.Add(cr, new ControllPlaceBranch(){tvi = new TreeViewItem(){Header = cr.GetName()}, ctmPostBranches = null});
            UpdateTree();
        }
        else if (os is LCS lcs)
        {
            TreeViewItem tvi = new TreeViewItem() { Header = lcs.GetName(), IsExpanded = true};
            _drawableControlPlaces.Add(lcs, new ControllPlaceBranch() { tvi = tvi, ctmPostBranches = new List<CtmPostBranch>() });
            foreach (Post post in lcs.GetPosts())
            {
                TreeViewItem tviPost = new TreeViewItem() { Header = post.GetName() };
                _drawableControlPlaces[lcs].ctmPostBranches!.Add(new CtmPostBranch() { post = post, tvi = tviPost });
                tvi.Items.Add(tviPost);
            }
            UpdateTree();
        }
    }

    public void OnControlPlaceChanged(object? sender, EventArgs eventArgs)
    {
        throw new NotImplementedException();
    }
    
    public void UpdateTree()
    {
        _treeView.Items.Clear();
        foreach (var (_, value) in _drawableControlPlaces)
        {
            if (value.ctmPostBranches == null)
            {
                _treeView.Items.Add(value.tvi);
                continue;
            }
            TreeViewItem tvi = value.tvi;
            tvi.Items.Clear();
            foreach (CtmPostBranch cpb in value.ctmPostBranches) tvi.Items.Add(cpb.tvi);
            _treeView.Items.Add(tvi);
        }
    }
}