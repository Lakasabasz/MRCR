using System;
using System.Collections.Generic;
using System.Linq;

namespace MRCR.datastructures;

public class NeighbourMatrix
{
    private Trail?[] _mx;
    private UniqueList<Post?> _dict;

    public NeighbourMatrix()
    {
        _mx = Array.Empty<Trail?>();
        _dict = new UniqueList<Post?>();
    }
    public NeighbourMatrix(List<Post> posts)
    {
        int n = posts.Count;
        _mx = new Trail?[n*(n-1)/2];
        Array.Fill(_mx, null);
        _dict = new UniqueList<Post?>();
        foreach (Post post in posts)
        {
            _dict.Add(post);
        }
    }
    public List<Trail> GetTrailsList()
    {
        List<Trail> tlist = new();
        List<int> indexes = _dict.Where(x => x != null).Select(x => _dict.IndexOf(x)).ToList();
        for (int i = 0; i < indexes.Count; i++)
        {
            for(int j = i+1; j < indexes.Count; j++)
            {
                Trail? t = _mx[j * (j - 1) / 2 + i];
                if(t != null)
                {
                    tlist.Add(t);
                }
            }
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
            if (value != null && !(value.Contains(postA) && value.Contains(postB)))
                throw new ArgumentException("Trail have to contain both posts");
            int x = _dict[postA] > _dict[postB] ? _dict[postB] : _dict[postA];
            int y = _dict[postA] > _dict[postB] ? _dict[postA] : _dict[postB];
            int i = y*(y-1)/2 + x;
            _mx[i] = value;
        }
    }
    
    public Dictionary<Post, Trail?>? this[Post post]
    {
        get
        {
            if (!_dict.Contains(post)) throw new IndexOutOfRangeException();
            List<Post> posts = _dict.Where(x => x != null && !Equals(x, post)).ToList()!;
            Dictionary<Post, Trail?> dict = new Dictionary<Post, Trail?>();
            foreach (var p in posts)
            {
                dict.Add(p, this[post, p]);
            }
            return dict;
        }
        set
        {
            if (value == null)
            {
                value = new Dictionary<Post, Trail?>();
                foreach(var p in _dict.Where(x => x != null && !Equals(x, post)))
                {
                    value.Add(p!, null);
                }
            }
            if (!_dict.Contains(post)) throw new IndexOutOfRangeException();
            foreach (var p in value.Keys)
            {
                if (!_dict.Contains(p)) throw new IndexOutOfRangeException();
                if (value[p] != null && !(value[p].Contains(p) && value[p].Contains(post)))
                    throw new ArgumentException("Trail have to contain both posts");
            }
            foreach (var (key, trail) in value)
            {
                this[key, post] = trail;
            }
        }
    }

    public void AddPost(Post post)
    {
        int n = _dict.Count;
        _dict.Add(post);
        bool resized = _dict.Count != n;
        if (resized)
        {
            Trail?[] newMx = new Trail?[n*(n+1)/2];
            Array.Fill(newMx, null);
            Array.Copy(_mx, newMx, _mx.Length);
            _mx = newMx;
            return;
        }
        List<int> neighbors = _dict.Where(x => x != null && x.Equals(post)).Select(x => _dict[x]).ToList();
        foreach (int neighbor in neighbors)
        {
            int i;
            if (neighbor > _dict[post])
            {
                i = neighbor * (neighbor - 1) / 2 + _dict[post];
            }
            else
            {
                i = _dict[post] * (_dict[post] - 1) / 2 + neighbor;
            }
            _mx[i] = null;
        }
    }

    public bool VerifiConsistency(List<Post>? posts = null)
    {
        posts ??= _dict.Where(x => x != null).ToList();
        List<Post> discoveredPosts = new List<Post>(posts.Count);
        discoveredPosts.Add(posts[0]);
        var neighbors = this[posts[0]];
        foreach(var (post, trail) in neighbors)
        {
            if(posts.Contains(post) && trail != null) discoveredPosts.Add(post);
        }

        for (int i = 1; i < discoveredPosts.Count; i++)
        {
            if(discoveredPosts.Count == posts.Count) return true;
            neighbors = this[discoveredPosts[i]];
            foreach (var (post, trail) in neighbors)
            {
                if (!posts.Contains(post) || trail == null) continue;
                if (!discoveredPosts.Contains(post)) discoveredPosts.Add(post);
            }
        }
        return discoveredPosts.Count == posts.Count;
    }

    public NeighbourMatrix GetSubgraph(List<Post> posts)
    {
        NeighbourMatrix subgraph = new NeighbourMatrix(posts);
        for (var i = 0; i < posts.Count; i++)
        {
            for (var j = i+1; j < posts.Count; j++)
            {
                subgraph[posts[i], posts[j]] = this[posts[i], posts[j]];
            }
        }
        return subgraph;
    }

    public int GetPostsCount()
    {
        return _dict.Count(x => x != null);
    }
}