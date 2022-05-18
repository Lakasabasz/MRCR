using System.Collections.Generic;
using MRCR.datastructures;

namespace MRCR.services;

public class ObjectsSelectionService: ISelectionService<Post>
{
    List<Post> _selectedObjects = new();
    public void Set(List<Post> items)
    {
        if(_selectedObjects.Count > 0)
        {
            foreach(Post item in _selectedObjects)
            {
                item.IsSelected = false;
            }
        }
        items.ForEach(item => item.IsSelected = true);
        _selectedObjects = items;
    }

    public List<Post> Get()
    {
        return _selectedObjects;
    }

    public void Add(Post item)
    {
        item.IsSelected = true;
        _selectedObjects.Add(item);
    }

    public void Add(List<Post> items)
    {
        foreach (Post post in items)
        {
            post.IsSelected = true;
        }
        _selectedObjects.AddRange(items);
    }

    public void Remove(Post item)
    {
        item.IsSelected = false;
        _selectedObjects.Remove(item);
    }

    public void Remove(List<Post> items)
    {
        items.ForEach(x => { _selectedObjects.Remove(x); x.IsSelected = false; });
    }
}