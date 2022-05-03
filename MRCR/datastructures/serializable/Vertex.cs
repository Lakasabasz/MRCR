using System;

namespace MRCR.datastructures.serializable;

public class Vertex
{
    [NonSerialized]
    private static int _lastId = 0;

    public int Id { get; set; } = 0;
    public string Name { get; set; } = "";
    public int Type { get; set; } = 0;
    
    public Vertex(){} // From deserialization

    public Vertex(string name, int type)
    {
        Id = _lastId;
        _lastId++;
        Name = name;
        Type = type;
    }
}
