using System.Collections.Generic;

namespace MRCR.datastructures;

public class World : IValidable
{
    public string Name { get; set; }
    public List<Post> Posts { get; set; }
    public List<Line> Lines { get; set; }
    public List<ControlPlace> ControlPlaces { get; set; }
    public static World Load(string worldPath)
    {
        throw new System.NotImplementedException();
    }
    public void Save()
    {
        throw new System.NotImplementedException();
    }
    public bool IsValid()
    {
        throw new System.NotImplementedException();
    }
}