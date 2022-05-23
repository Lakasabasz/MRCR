using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using MRCR.datastructures;

namespace MRCR.Editor;

class TrailBranch: Dictionary<Trail, TreeViewItem> { }

class OttmPostBranch
{
    public TreeViewItem TreeViewItem { get; set; }
    public TrailBranch TrailBranches { get; set; }
}

class ObjectsTrailsTreeManager : ITreeManager
{
    private World _world;
    private Dictionary<Post, OttmPostBranch> _treeItemsRoot;
    private TreeView _treeView;
    
    public ObjectsTrailsTreeManager(World w, TreeView treeView)
    {
        _world = w;
        _world.RegisterDelegate(OrganisationObjectType.Post, OnObjectsAndTrailsChanged);
        _world.OnWorldStateChanged += OnWorldStateChanged;
        _treeItemsRoot = new Dictionary<Post, OttmPostBranch>();
        _treeView = treeView;
    }

    private void OnObjectsAndTrailsChanged(object? sender, EventArgs e)
    {
        // if (sender is not Post p) return; 
        // _treeItemsRoot[p].TreeViewItem.IsSelected = p.IsSelected;
        // _treeItemsRoot[p].TreeViewItem.Header = p.GetName();
    }
    
    private void OnWorldStateChanged(object? sender, IOrganizationStructure e)
    {
        if (e is Post post)
        {
            _treeItemsRoot.Add(post, new OttmPostBranch()
            {
                TreeViewItem = new TreeViewItem(){Header = post.GetName()},
                TrailBranches = new TrailBranch()
            });
            UpdateTree();
        }
        else if (e is Trail trail)
        {
            Post[] posts = trail.GetPosts();
            _treeItemsRoot[posts[0]].TrailBranches.Add(trail, new TreeViewItem(){Header = posts[1].GetName()});
            _treeItemsRoot[posts[1]].TrailBranches.Add(trail, new TreeViewItem(){Header = posts[0].GetName()});
            UpdateTree();
        }
    }

    public void UpdateTree()
    {
        _treeView.Items.Clear();
        foreach (var (_, postBranch) in _treeItemsRoot)
        {
            TreeViewItem postTvi = postBranch.TreeViewItem;
            postTvi.Items.Clear();
            foreach (var (_, trailTvi) in postBranch.TrailBranches)
            {
                postTvi.Items.Add(trailTvi);
            }
            _treeView.Items.Add(postTvi);
        }
    }

    public void UpdateSelection(TreeViewItem old, TreeViewItem newer)
    {
        Post? p = null;
        foreach (var (key, value) in _treeItemsRoot)
        {
            if(value.TreeViewItem == newer)
            {
                p = key;
                break;
            }
        }
        if (p == null) return;
        ((ISelectionService<Post>)_world.SelectionServices[OrganisationObjectType.Post]).Set(new List<Post>{p});
    }
}