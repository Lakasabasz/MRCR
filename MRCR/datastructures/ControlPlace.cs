using System;
using System.Collections.Generic;

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
        throw new NotImplementedException();
    }
}

public class LCS : IControlPlace
{
    private List<Post> _posts;
    private string _name;
    
    public LCS(List<Post> posts, string name)
    {
        _name = name;
        _posts = posts;
    }

    public List<Post> GetPosts()
    {
        return _posts;
    }

    public string GetName()
    {
        return _name;
    }

    public void AddPost(Post p3)
    {
        throw new NotImplementedException();
    }
}