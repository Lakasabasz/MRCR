﻿using System;

namespace MRCR.datastructures;

public class Trail: IOrganizationStructure
{
    private Post _refA;
    private Post _refB;

    public Trail(Post refA, Post refB)
    {
        _refA = refA;
        _refB = refB;
    }
    public Post[] GetPosts()
    {
        Post[] posts = new Post[2];
        posts[0] = _refA;
        posts[1] = _refB;
        return posts;
    }
    public override bool Equals(object? obj)
    {
        Trail? other = obj as Trail;
        if (other == null)
        {
            return false;
        }
        return (_refA.Equals(other._refA) && _refB.Equals(other._refB)) || (_refA.Equals(other._refB) && _refB.Equals(other._refA));
    }
    protected bool Equals(Trail other)
    {
        return _refA.Equals(other._refA) && _refB.Equals(other._refB);
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(_refA, _refB);
    }
    
    public bool Contains(Post post)
    {
        return post.Equals(_refA) || post.Equals(_refB);
    }
    public Post Second(Post post)
    {
        if (_refA.Equals(post))
        {
            return _refB;
        }
        if(_refB.Equals(post))
        {
            return _refA;
        }
        throw new ArgumentException();
    }
    public OrganisationObjectType Type => OrganisationObjectType.Trail;
    public event EventHandler? OnPropertyChanged;
    public void RaisePropertyChanged()
    {
        OnPropertyChanged?.Invoke(this, EventArgs.Empty);
    }
}