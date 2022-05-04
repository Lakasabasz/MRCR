using System;
using System.Collections.Generic;

namespace MRCR.datastructures;

public class Line
{
    private List<Post> _posts;
    private List<Trail> _trails;
    private string _name;
    public Line(List<Post> linePosts, string name)
    {
        throw new NotImplementedException();
    }
    public List<Post> GetPosts()
    {
        return _posts;
    }

    public string GetName()
    {
        return _name;
    }

    public void AddPostAfter(Post p4)
    {
        throw new NotImplementedException();
    }
}