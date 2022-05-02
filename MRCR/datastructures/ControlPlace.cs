using System;
using System.Collections.Generic;

namespace MRCR.datastructures;

public class ControlPlace
{
    public int PostReference {get;set;}
}

public class LCS : ControlPlace
{
    public List<int> PostsReference {get;set;}
}