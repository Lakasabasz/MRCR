using System.Collections.Generic;

namespace MRCR.datastructures.serializable;

public class SerializableGraph
{
    public string name { get; set; }
    public List<Vertex> vertices { get; set; }
    public List<Edge> edges { get; set; }
    public List<NamedTree> lines { get; set; }
    public List<NamedSubgraph> subgraphs { get; set; }
}