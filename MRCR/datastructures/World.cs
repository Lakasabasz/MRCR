using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.Json;
using MRCR.datastructures.serializable;

namespace MRCR.datastructures;

public class World : IValidable
{
    private string _name;
    private List<Post> _posts;
    private TriangleMatrix<Trail?> _neighborsMatrix;
    private List<Line> _lines;
    private List<IControlPlace> _controlPlaces;

    public World(string name)
    {
        _name = name;
        _posts = new List<Post>();
        _neighborsMatrix = new TriangleMatrix<Trail?>();
        _lines = new List<Line>();
        _controlPlaces = new List<IControlPlace>();
    }

    private World(string name, List<Post> posts, List<Trail> trails, List<Line> lines, List<IControlPlace> controlPlaces)
    {
        throw new NotImplementedException();
    }

    public static World Load(string worldPath)
    {
        var worldData = File.ReadAllLines(worldPath);
        string worldDataString = string.Join("\n", worldData);
        SerializableGraph? sg = JsonSerializer.Deserialize<SerializableGraph>(worldDataString);
        if (sg == null) throw new Exception("Could not deserialize world data");
        
        List<Post> posts = new List<Post>();
        Dictionary<int, Post> idPosts = new Dictionary<int, Post>();
        foreach(Vertex v in sg.vertices)
        {
            posts.Add(new Post(v.Name, v.Type));
            idPosts.Add(v.Id, posts[^1]);
        }
        
        List<Trail> trails = new List<Trail>();
        foreach (Edge e in sg.edges)
        {
            Trail t = new Trail(posts[e.V1], posts[e.V2]);
            trails.Add(t);
            Post[] p = t.GetPosts();
            p[0].AddTrail(t);
            p[1].AddTrail(t);
        }
        
        List<Line> lines = new List<Line>();
        foreach (NamedTree l in sg.lines)
        {
            List<Post> linePosts = new List<Post>();
            foreach (int i in l.Tree)
            {
                if (linePosts.Count != 0)
                {
                    Post last = linePosts[^1];
                    if (last.GetTrailContaining(idPosts[i]) == null) throw new Exception("Line does not contain a trail");
                }
                linePosts.Add(idPosts[i]);
            }
            lines.Add(new Line(linePosts, l.Name));
        }
        
        List<IControlPlace> controlPlaces = new List<IControlPlace>();
        foreach (NamedSubgraph namedSubgraph in sg.subgraphs)
        {
            List<Post> subgraphPosts = new List<Post>();
            foreach (int i in namedSubgraph.Verices)
            {
                subgraphPosts.Add(idPosts[i]);
            }
            foreach (var subgraphPost in subgraphPosts.Skip(1))
            {
                if(!subgraphPosts[0].IsReachable(subgraphPost)) throw new Exception("Subgraph is not connected");
            }
            if(subgraphPosts.Count == 1)
            {
                controlPlaces.Add(new ControlRoom(subgraphPosts[0], namedSubgraph.Name));
            }
            else
            {
                controlPlaces.Add(new LCS(subgraphPosts, namedSubgraph.Name));
            }
        }
        
        World world = new World(sg.name, posts, trails, lines, controlPlaces);
        return world;
    }
    public string Save(string? filename)
    {
        throw new NotImplementedException();
    }
    public string Serialize()
    {
        throw new NotImplementedException();
    }
    public bool IsValid()
    {
        throw new NotImplementedException();
    }
}