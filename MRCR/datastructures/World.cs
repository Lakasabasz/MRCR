using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Windows;
using MRCR.datastructures.serializable;
using Point = System.Drawing.Point;

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
        foreach (var line in _lines)
        {
            line.SetWorld(this);
        }
        foreach (var controlPlace in _controlPlaces)
        {
            if(controlPlace is LCS lcs) lcs.SetWorld(this);
        }
    }

    public static World Load(string worldPath)
    {
        var worldData = File.ReadAllLines(worldPath);
        string worldDataString = string.Join("\n", worldData);
        SerializableGraph? sg;
        try
        {
            sg = JsonSerializer.Deserialize<SerializableGraph>(worldDataString);
        }
        catch (JsonException e)
        {
            Console.WriteLine(e);
            MessageBox.Show("Invalid world file", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            throw e;
        }
        if (sg == null) throw new Exception("Could not deserialize world data");
        
        List<Post> posts = new List<Post>();
        Dictionary<int, Post> idPosts = new Dictionary<int, Post>();
        foreach(Vertex v in sg.vertices)
        {
            posts.Add(new Post(v.Name, v.Type, v.X, v.Y));
            idPosts.Add(v.Id, posts[^1]);
        }
        
        List<Trail> trails = new List<Trail>();
        foreach (Edge e in sg.edges)
        {
            Trail t = new Trail(idPosts[e.V1], idPosts[e.V2]);
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
        SerializableGraph sg = new SerializableGraph
        {
            name = _name, vertices = vertices, edges = edges, lines = lines, subgraphs = subgraphs
        };
        return JsonSerializer.Serialize(sg);
    }
    public bool IsValid()
    {
        throw new NotImplementedException();
    }

    public Post AddPost(int x, int y, PostType type)
    {
        // Two posts cannot be on the same position
        Post? any = _posts.Find(p => p.GetPosition() == new Point(x, y));
        if (any != null)
        {
            throw new Exception("Two posts cannot be on the same position");
        }
        Post post = new Post(type, new Point(x, y));
        _neighborsMatrix.AddPost(post);
        _posts.Add(post);
        _controlPlaces.Add(new ControlRoom(post, post.GetName()));
        return post;
    }

    public void Connect(Post p1, Post p2)
    {
        // Two posts cannot be connected if they are already connected
        if(_neighborsMatrix[p1, p2] != null)
        {
            throw new Exception("Two posts cannot be connected if they are already connected");
        }
        Trail t = new Trail(p1, p2);
        _neighborsMatrix[p1, p2] = t;
        p1.AddTrail(t);
        p2.AddTrail(t);

        CreateLine(new List<Post> { p1, p2 });
    }

    public Line CreateLine(List<Post> posts)
    {
        Post? last = null;
        List<Trail> trails = new List<Trail>();
        foreach (Post p in posts)
        {
            if (last != null)
            {
                Trail? t = _neighborsMatrix[last, p];
                if(t == null)
                {
                    throw new Exception("Posts are not connected");
                }
                trails.Add(t);
            }
            last = p;
        }

        List<Line> overlappingLines = new List<Line>();
        foreach (Line l in _lines)
        {
            if (!l.ContainsAnyOf(trails)) continue;
            if (l.GetPosts().Count > 2)
            {
                throw new Exception("Line overlapping with another line");
            }
            overlappingLines.Add(l);
        }
        _lines.RemoveAll(x => overlappingLines.Contains(x));
        Line line = new Line(posts, trails, this);
        _lines.Add(line);
        return line;
    }

    public LCS CreateLCS(List<Post> posts)
    {
        if(!_neighborsMatrix.VerifiConsistency(posts)) throw new Exception("Posts are not connected");
        NeighbourMatrix subgraph = _neighborsMatrix.GetSubgraph(posts);
        foreach(IControlPlace cp in _controlPlaces)
        {
            LCS? tlcs = cp as LCS;
            if (tlcs == null) continue;
            if(tlcs.ContainsAnyOf(posts))
            {
                throw new Exception("Other LCS contains posts of this LCS");
            }
        }
        LCS lcs = new LCS(posts, subgraph,this);
        for (var i = _controlPlaces.Count - 1; i >= 0; i--)
        {
            ControlRoom? cr = _controlPlaces[i] as ControlRoom;
            if (cr == null) continue;
            if (lcs.ContainsAnyOf(cr.GetPosts()))
            {
                _controlPlaces.RemoveAt(i);
            }
        }
        _controlPlaces.Add(lcs);
        return lcs;
    }

    public void ExpandLine(Line line, Trail trail)
    {
        Line? overlapping = null;
        foreach (var l in _lines)
        {
            if (!l.ContainsAnyOf(new List<Trail> { trail })) continue;
            if (l.GetPosts().Count > 2)
            {
                throw new Exception("Line overlapping with another line");
            }
            overlapping = l;
        }
        _lines.Remove(overlapping);
    }

    public void ExpandLCS(LCS lcs, Post post)
    {
        ControlRoom? overlapping = null;
        foreach (var cr in _controlPlaces)
        {
            if (!cr.GetPosts().Contains(post)) continue;
            if(cr is LCS) throw new Exception("LCS overlaps another LCS");
            overlapping = cr as ControlRoom;
        }
        _controlPlaces.Remove(overlapping);
    }
}