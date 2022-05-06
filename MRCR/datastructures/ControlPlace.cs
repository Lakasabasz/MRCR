using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MRCR.datastructures;

public interface IControlPlace
{
    List<Post> GetPosts();
    string GetName();
}

public class ControlRoom : IControlPlace
{
    private Post _post;
    private string _name;
    
    public ControlRoom(Post post, string name)
    {
        _name = name;
        _post = post;
    }
    public List<Post> GetPosts()
    {
        return new List<Post>{ _post };
    }
    public string GetName()
    {
        return _name;
    }
}

public class LCS : IControlPlace
{
    private List<Post> _posts;
    private string _name;
    private NeighbourMatrix _neighbourMatrix;
    private World? _world = null;
    private static int _namelessCounter = 1;
    
    public LCS(List<Post> posts, string name)
    {
        _name = name;
        _posts = posts;
        _neighbourMatrix = new NeighbourMatrix(posts);
    }
    public LCS(List<Post> posts, NeighbourMatrix neighbours, World world, string? name = null)
    {
        name ??= $"LCS {_namelessCounter++}";
        _name = name;
        _posts = posts;
        _neighbourMatrix = neighbours;
        _world = world;
    }

    public List<Post> GetPosts()
    {
        return _posts;
    }

    public string GetName()
    {
        return _name;
    }

    public void AddPost(Post post)
    {
        Dictionary<Post, Trail?> trails = new();
        foreach (Post neighbour in _posts)
        {
            trails.Add(neighbour, neighbour.GetTrailContaining(post));
        }
        if (trails.Count(x => x.Value != null) == 0)
        {
            throw new Exception("Post is not connected to any other posts");
        }

        _world.ExpandLCS(this, post);
        _posts.Add(post);
        _neighbourMatrix.AddPost(post);
        _neighbourMatrix[post] = trails;
    }

    public bool ContainsAnyOf(List<Post> posts)
    {
        return _posts.Any(x => posts.Contains(x));
    }

    public void SetWorld(World world)
    {
        _world = world;
    }
}