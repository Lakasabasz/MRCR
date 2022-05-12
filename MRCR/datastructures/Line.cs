using System;
using System.Collections.Generic;
using System.Linq;

namespace MRCR.datastructures;

public class Line : IOrganizationStructure
{
    private List<Post> _posts;
    private List<Trail> _trails;
    private string _name;
    private static int _namelessCounter = 0;
    private World? _world;

    public Line(List<Post> linePosts, string name)
    {
        _posts = linePosts;
        _name = name;
        Post? last = null;
        _trails = new List<Trail>();
        foreach (Post post in _posts)
        {
            if (last != null)
            {
                Trail? t = post.GetTrailContaining(last);
                if (t != null)
                {
                    _trails.Add(t);
                }
            }
            last = post;
        }
    }

    public Line(List<Post> linePosts, List<Trail> trails, World world, string? name = null)
    {
        if(name == null)
        {
            name = $"Linia {_namelessCounter + 1}";
            _namelessCounter++;
        }
        _posts = linePosts;
        _trails = trails;
        _name = name;
        _world = world;
    }
    public List<Post> GetPosts()
    {
        return _posts;
    }
    public void SetWorld(World w)
    {
        _world = w;
    }
    public string GetName()
    {
        return _name;
    }

    public void AddPostAfter(Post post)
    {
        Trail? t = _posts[^1].GetTrailContaining(post);
        if (t == null) throw new Exception("Post has not trail connecting it to the line end");
        if(_world == null) throw new Exception("Line has no world");
        _world.ExpandLine(this, t);
        _posts.Add(post);
        _trails.Add(t);
    }

    public bool ContainsAnyOf(List<Trail> trails)
    {
        return trails.Any(trail => _trails.Contains(trail));
    }
}