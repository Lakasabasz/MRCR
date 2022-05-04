using System;
using System.Collections.Generic;

namespace MRCR.datastructures;

public class NeighbourMatrix
{
    private Trail?[] _mx;
    private Dictionary<Post, int> _dict;

    public NeighbourMatrix()
    {
        _mx = Array.Empty<Trail?>();
        _dict = new Dictionary<Post, int>();
    }
    public NeighbourMatrix(List<Post> posts)
    {
        int n = posts.Count;
        _mx = new Trail?[n*(n-1)/2];
        Array.Fill(_mx, null);
        _dict = new Dictionary<Post, int>(posts.Count);
        int id = 0;
        foreach (Post p in posts)
        {
            _dict.Add(p, id);
            id++;
        }
    }
    public List<Trail> GetTrailsList()
    {
        List<Trail> tlist = new();
        foreach (Trail? x in _mx)
        {
            if(x == null) continue;
            tlist.Add(x);
        }
        return tlist;
    }

    public Trail? this[Post postA, Post postB]
    {
        get
        {
            int x = _dict[postA] > _dict[postB] ? _dict[postB] : _dict[postA];
            int y = _dict[postA] > _dict[postB] ? _dict[postA] : _dict[postB];
            int i = y*(y-1)/2 + x;
            return _mx[i];
        }
        set
        {
            int x = _dict[postA] > _dict[postB] ? _dict[postB] : _dict[postA];
            int y = _dict[postA] > _dict[postB] ? _dict[postA] : _dict[postB];
            int i = y*(y-1)/2 + x;
            _mx[i] = value;
        }
    }
}