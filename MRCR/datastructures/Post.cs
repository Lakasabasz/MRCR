using System;
using System.Collections.Generic;

namespace MRCR.datastructures;

public enum PostType
{
    Post, Depot, Combined
}
public class Post
{
    private string _name;
    private PostType _type;
    private List<Trail> _trails;

    public Post(string name, int type)
    {
        _name = name;
        switch (type)
        {
            case 0:
                _type = PostType.Post;
                break;
            case 1:
                _type = PostType.Depot;
                break;
            case 2:
                _type = PostType.Combined;
                break;
            default:
                throw new ArgumentException();
        }
        _trails = new List<Trail>();
    }
    public void AddTrail(Trail trail)
    {
        if (!trail.Contains(this))
        {
            throw new ArgumentException();
        }
        _trails.Add(trail);
    }

    public override bool Equals(object? obj)
    {
        Post? other = obj as Post;
        if (other == null)
        {
            return false;
        }
        return _name == other._name && _type == other._type;
    }
    public Trail? GetTrailContaining(Post idPost)
    {
        return _trails.Find(t => t.Contains(idPost));
    }

    public bool IsReachable(Post post, List<Post>? before = null)
    {
        (before ??= new List<Post>()).Add(this);
        if (Equals(post)) return true;
        foreach (var trail in _trails)
        {
            if (before.Contains(trail.Second(this))) continue;
            if (trail.Second(this).IsReachable(post, before)) return true;
        }

        return false;
    }
}