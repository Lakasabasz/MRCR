using System;
using System.Windows;

namespace MRCR.datastructures.serializable;

public class Vertex
{
    [NonSerialized]
    private static int _lastId = 0;

    public int Id { get; set; } = 0;
    public string Name { get; set; } = "";
    public int Type { get; set; } = 0;
    
    public Vertex(){} // From deserialization

    public Vertex(string name, int type, int x, int y)
    {
        Id = _lastId;
        _lastId++;
        Name = name;
        Type = type;
        X = x;
        Y = y;
    }

    public int Y { get; set; }
    public int X { get; set; }

    public static void ResetCounters()
    {
        _lastId = 0;
    }
}
