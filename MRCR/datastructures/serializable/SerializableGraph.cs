using System.Collections.Generic;

namespace MRCR.datastructures.serializable;

public class SerializableGraph
{
    public string name;
    public List<Vertex> vertices;
    public List<Edge> edges;
    public List<NamedTree> lines;
    public List<NamedSubgraph> subgraphs;
}