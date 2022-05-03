using System;
using System.Collections.Generic;

namespace MRCR.datastructures;

public interface IControlPlace {}

public class ControlRoom : IControlPlace
{
    private Post _post;
    private string _name;
    
    public ControlRoom(Post post, string name)
    {
        _name = name;
        _post = post;
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
}