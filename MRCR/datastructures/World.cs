using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using MRCR.datastructures.serializable;

namespace MRCR.datastructures;

public class World : IValidable
{
    private string _name;
    private List<Post> _posts;
    private NeighbourMatrix _neighborsMatrix;
    private List<Line> _lines;
    private List<IControlPlace> _controlPlaces;

    public World(string name)
    {
        _name = name;
        _posts = new List<Post>();
        _neighborsMatrix = new NeighbourMatrix();
        _lines = new List<Line>();
        _controlPlaces = new List<IControlPlace>();
    }

    private World(string name, List<Post> posts, List<Trail> trails, List<Line> lines, List<IControlPlace> controlPlaces)
    {
        _name = name;
        _posts = posts;
        _lines = lines;
        _controlPlaces = controlPlaces;
        _neighborsMatrix = new NeighbourMatrix(_posts);
        foreach (Trail trail in trails)
        {
            _neighborsMatrix[trail.GetPosts()[0], trail.GetPosts()[1]] = trail;
        }
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
    public void Save(string? filename)
    {
        UTF8Encoding utf8 = new UTF8Encoding(true);
        string json = Serialize();
        filename ??= Config.WorldDirectoryPath + _name + Config.WorldFileExtension;
        FileStream file;
        if (File.Exists(filename))
        {
            file = File.Open(filename, FileMode.Truncate);
        }
        else
        {
            file = File.Create(filename);
        }
        file.Write(utf8.GetBytes(json), 0, utf8.GetByteCount(json));
        file.Close();
    }
    public string Serialize()
    {
        List<Vertex> vertices = new List<Vertex>();
        Dictionary<Post, int> postIds = new Dictionary<Post, int>();
        foreach (Post p in _posts)
        {
            vertices.Add(p.ToVertex());
            postIds.Add(p, vertices[^1].Id);
        }
        
        List<Edge> edges = new List<Edge>();
        List<Trail> trails = _neighborsMatrix.GetTrailsList();
        foreach (Trail t in trails)
        {
            edges.Add(new Edge{V1 = postIds[t.GetPosts()[0]], V2 = postIds[t.GetPosts()[1]]});;
        }
        
        List<NamedTree> lines = new List<NamedTree>();
        foreach (Line line in _lines)
        {
            List<int> lineVertices = new List<int>();
            foreach (Post p in line.GetPosts())
            {
                lineVertices.Add(postIds[p]);
            }
            NamedTree nt = new NamedTree{Name = line.GetName(), Tree = lineVertices};
            lines.Add(nt);
        }
        
        List<NamedSubgraph> subgraphs = new List<NamedSubgraph>();
        foreach (IControlPlace controlPlace in _controlPlaces)
        {
            List<Post> controlPlacePosts = controlPlace.GetPosts();
            List<int> controlPlaceVertices = new List<int>();
            foreach (Post p in controlPlacePosts)
            {
                controlPlaceVertices.Add(postIds[p]);
            }

            NamedSubgraph ns = new NamedSubgraph { Name = controlPlace.GetName(), Verices = controlPlaceVertices };
            subgraphs.Add(ns);
        }
        SerializableGraph sg = new SerializableGraph { name = _name, vertices = vertices, edges = edges, lines = lines, subgraphs = subgraphs };
        return JsonSerializer.Serialize(sg);
    }
    public bool IsValid()
    {
        throw new NotImplementedException();
    }

    public Post AddPost(int x, int y, PostType type)
    {
        throw new NotImplementedException();
    }

    public void Connect(Post p1, Post p2)
    {
        throw new NotImplementedException();
    }

    public Line CreateLine(List<Post> posts)
    {
        throw new NotImplementedException();
    }

    public LCS CreateLCS(List<Post> posts)
    {
        throw new NotImplementedException();
    }
}