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
public class Post: IOrganizationStructure
{
    private string Name
    {
        get => _name;
        set{
            _name = value;
            RaisePropertyChanged();
        }
    }
    private Point Location
    {
        get => _location;
        set{
            _location = value;
            RaisePropertyChanged();
        }
    }

    private PostType _type;
    private List<Trail> _trails;
    private string _name;
    private Point _location;

    private static int _namelessCounterPost = 0;
    private static int _namelessCounterDepot = 0;
    private static int _namelessCounterCombined = 0;

    public Post(string name, int type, int x, int y)
    {
        Name = name;
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
                Name = "Posterunek " + (_namelessCounterPost + 1);
                _namelessCounterPost++;
                break;
            case PostType.Depot:
                Name = "Lokomotywownia " + (_namelessCounterDepot + 1);
                _namelessCounterDepot++;
                break;
            case PostType.Combined:
                Name = "Stacja " + (_namelessCounterCombined + 1);
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
        return Name == other.Name && _type == other._type;
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
        return new Vertex(Name, (int)_type, Location.X, Location.Y);
    }

    public Point GetPosition()
    {
        return Location;
    }

    public void SetName(string name)
    {
        Name = name;
    }

    public string GetName()
    {
        return Name;
    }

    public static void ResetCounters()
    {
        _namelessCounterCombined = 0;
        _namelessCounterDepot = 0;
        _namelessCounterPost = 0;
    }
    public OrganisationObjectType Type => OrganisationObjectType.Post;
    
    public bool IsSelected { 
        get => _isSelected;
        set
        {
            _isSelected = value;
            RaisePropertyChanged();
        }
    }
    private bool _isSelected;

    public event EventHandler? OnPropertyChanged;
    private void RaisePropertyChanged()
    {
        OnPropertyChanged?.Invoke(this, EventArgs.Empty);
    }
}