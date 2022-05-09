using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using MRCR.datastructures.serializable;

namespace MRCR.datastructures;

public enum PostType
{
    Post = 0,
    Depot = 1,
    Combined = 2
}
public class Post
{
    private string _name;
    private PostType _type;
    private List<Trail> _trails;
    private Point _location;
    
    private static int _namelessCounterPost = 0;
    private static int _namelessCounterDepot = 0;
    private static int _namelessCounterCombined = 0;

    public Post(string name, int type, int x, int y)
    {
        _name = name;
        _type = (PostType)type;
        _trails = new List<Trail>();
        _location = new Point(x, y);
    }
    public Post(PostType type, Point location)
    {
        _trails = new List<Trail>();
        _type = type;
        switch (type)
        {
            case PostType.Post:
                _name = "Posterunek " + (_namelessCounterPost + 1);
                _namelessCounterPost++;
                break;
            case PostType.Depot:
                _name = "Lokomotywownia " + (_namelessCounterDepot + 1);
                _namelessCounterDepot++;
                break;
            case PostType.Combined:
                _name = "Stacja " + (_namelessCounterCombined + 1);
                _namelessCounterCombined++;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }

        _location = location;
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

    public Vertex ToVertex()
    {
        return new Vertex(_name, (int)_type, _location.X, _location.Y);
    }

    public Point GetPosition()
    {
        return _location;
    }

    public void SetName(string name)
    {
        _name = name;
    }

    public string GetName()
    {
        return _name;
    }

    public static void ResetCounters()
    {
        _namelessCounterCombined = 0;
        _namelessCounterDepot = 0;
        _namelessCounterPost = 0;
    }
}