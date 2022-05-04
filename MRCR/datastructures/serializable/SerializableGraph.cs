using System.Collections.Generic;

namespace MRCR.datastructures.serializable;

public class SerializableGraph
{
    public string name;
    public List<Vertex> vertices = new();
    public List<Edge> edges = new();
    public List<NamedTree> lines = new();
    public List<NamedSubgraph> subgraphs = new();
}