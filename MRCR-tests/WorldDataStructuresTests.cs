using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using MRCR.datastructures;
using MRCR.datastructures.serializable;
using NUnit.Framework;

namespace MRCR_tests;

public class WorldDataStructuresTests
{
    [Test, NonParallelizable, Order(1)]
    public void CreateTest()
    {
        Directory.CreateDirectory("test_data");
        World w = new World("TestWorld");
        w.Save("test_data/TestWorld.json");
        Assert.IsTrue(File.Exists("test_data/TestWorld.json"));
        World w2 = World.Load("test_data/TestWorld.json");
        SerializableGraph sg1 = JsonSerializer.Deserialize<SerializableGraph>(w.Serialize());
        SerializableGraph sg2 = JsonSerializer.Deserialize<SerializableGraph>(w2.Serialize());
        Assert.AreEqual(sg1.name, sg2.name);
    }
    
    [Test, NonParallelizable, Order(2)]
    public void NonEmptyWorldTest()
    {
        World w = new World("TestWorld");
        Post p1 = w.AddPost(0, 0, PostType.Combined);
        Post p2 = w.AddPost(1, 1, PostType.Post);
        Post p3 = w.AddPost(2, 2, PostType.Post);
        Post p4 = w.AddPost(3, 3, PostType.Post);
        Post d1 = w.AddPost(3, 2, PostType.Depot);
        w.Connect(p1, p2);
        w.Connect(p2, p3);
        w.Connect(p3, p4);
        w.Connect(p4, d1);
        Line l = w.CreateLine(new List<Post>{p1, p2, p3});
        l.AddPostAfter(p4);
        LCS lcs = w.CreateLCS(new List<Post> { p1, p2 });
        lcs.AddPost(p3);
        Directory.CreateDirectory("test_data");
        string serialized = w.Serialize();
        SerializableGraph sg = JsonSerializer.Deserialize<SerializableGraph>(serialized);

        {
            Dictionary<Vertex, bool> expected = new Dictionary<Vertex, bool>();
            expected.Add(new Vertex("Stacja 1", 2, 0, 0), false);
            expected.Add(new Vertex("Posterunek 1", 0, 1, 1), false);
            expected.Add(new Vertex("Posterunek 2", 0, 2, 2), false);
            expected.Add(new Vertex("Posterunek 3", 0, 3, 3), false);
            expected.Add(new Vertex("Lokomotywownia 1", 1, 3, 2), false);
            Assert.AreEqual(5, sg.vertices.Count);
            foreach (Vertex v in sg.vertices)
            {
                foreach(Vertex ev in expected.Keys){
                    if(v.Name == ev.Name && v.Type == ev.Type && v.X == ev.X && v.Y == ev.Y){
                        expected[ev] = true;
                        break;
                    }
                }
            }
            foreach(Vertex v in expected.Keys){
                Assert.IsTrue(expected[v]);
            }
        
            Assert.AreEqual(4, sg.edges.Count);
            Dictionary<Edge, bool> expectedEdges = new Dictionary<Edge, bool>();
            expectedEdges.Add(new Edge
            {
                V1 = sg.vertices.Find(vertex => vertex.Name == "Stacja 1").Id,
                V2 = sg.vertices.Find(vertex => vertex.Name == "Posterunek 1").Id
            }, false);
            expectedEdges.Add(new Edge
            {
                V1 = sg.vertices.Find(vertex => vertex.Name == "Posterunek 1").Id,
                V2 = sg.vertices.Find(vertex => vertex.Name == "Posterunek 2").Id
            }, false);
            expectedEdges.Add(new Edge
            {
                V1 = sg.vertices.Find(vertex => vertex.Name == "Posterunek 2").Id,
                V2 = sg.vertices.Find(vertex => vertex.Name == "Posterunek 3").Id
            }, false);
            expectedEdges.Add(new Edge
            {
                V1 = sg.vertices.Find(vertex => vertex.Name == "Posterunek 3").Id,
                V2 = sg.vertices.Find(vertex => vertex.Name == "Lokomotywownia 1").Id
            }, false);
            foreach(Edge e in sg.edges){
                foreach(Edge ee in expectedEdges.Keys){
                    if(e.V1 == ee.V1 && e.V2 == ee.V2 || e.V1 == ee.V2 && e.V2 == ee.V1){
                        expectedEdges[ee] = true;
                        break;
                    }
                }
            }
            foreach(Edge e in expectedEdges.Keys){
                Assert.IsTrue(expectedEdges[e]);
            }
        
            Assert.AreEqual(3, sg.lines.Count);
            Dictionary<NamedTree, bool> expectedLines = new Dictionary<NamedTree, bool>();
            expectedLines.Add(new NamedTree{Name = "Linia 1", Tree = new List<int>{
                sg.vertices.Find(vertex => vertex.Name == "Stacja 1").Id,
                sg.vertices.Find(vertex => vertex.Name == "Posterunek 1").Id,
                sg.vertices.Find(vertex => vertex.Name == "Posterunek 2").Id,
                sg.vertices.Find(vertex => vertex.Name == "Posterunek 3").Id
            }}, false);
            expectedLines.Add(new NamedTree{Name = "Linia 3", Tree = new List<int>{
                sg.vertices.Find(vertex => vertex.Name == "Posterunek 3").Id,
                sg.vertices.Find(vertex => vertex.Name == "Lokomotywownia 1").Id
            }}, false);
            foreach(NamedTree nt in sg.lines){
                foreach(NamedTree ent in expectedLines.Keys){
                    if(nt.Name == ent.Name && nt.Tree == ent.Tree){
                        expectedLines[ent] = true;
                        break;
                    }
                }
            }
            foreach(NamedTree nt in expectedLines.Keys){
                Assert.IsTrue(expectedLines[nt]);
            }
        
            Dictionary<NamedSubgraph, bool> expectedSubgraphs = new Dictionary<NamedSubgraph, bool>();
            expectedSubgraphs.Add(new NamedSubgraph{Name = "LCS1", Verices = new List<int>
            {
                sg.vertices.Find(vertex => vertex.Name == "Stacja 1").Id,
                sg.vertices.Find(vertex => vertex.Name == "Posterunek 1").Id,
                sg.vertices.Find(vertex => vertex.Name == "Posterunek 2").Id
            }}, false);
            expectedSubgraphs.Add(new NamedSubgraph{Name = "LCS2", Verices = new List<int>
            {
                sg.vertices.Find(vertex => vertex.Name == "Posterunek 3").Id
            }}, false);
            expectedSubgraphs.Add(new NamedSubgraph{Name = "LCS3", Verices = new List<int>
            {
                sg.vertices.Find(vertex => vertex.Name == "Lokomotywownia 1").Id
            }}, false);
            foreach(NamedSubgraph ns in sg.subgraphs){
                foreach(NamedSubgraph ens in expectedSubgraphs.Keys){
                    if(ns.Name == ens.Name && ns.Verices == ens.Verices){
                        expectedSubgraphs[ens] = true;
                        break;
                    }
                }
            }
            foreach(NamedSubgraph ns in expectedSubgraphs.Keys){
                Assert.IsTrue(expectedSubgraphs[ns]);
            }
        }
        
        World w2 = World.Load(serialized);
        SerializableGraph sg2 = JsonSerializer.Deserialize<SerializableGraph>(w2.Serialize());
        {
            Dictionary<Vertex, bool> expected = new Dictionary<Vertex, bool>();
            expected.Add(new Vertex("Stacja 1", 2, 0, 0), false);
            expected.Add(new Vertex("Posterunek 1", 0, 1, 1), false);
            expected.Add(new Vertex("Posterunek 2", 0, 2, 2), false);
            expected.Add(new Vertex("Posterunek 3", 0, 3, 3), false);
            expected.Add(new Vertex("Lokomotywownia 1", 1, 3, 2), false);
            Assert.AreEqual(5, sg2.vertices.Count);
            foreach (Vertex v in sg2.vertices)
            {
                foreach(Vertex ev in expected.Keys){
                    if(v.Name == ev.Name && v.Type == ev.Type && v.X == ev.X && v.Y == ev.Y){
                        expected[ev] = true;
                        break;
                    }
                }
            }
            foreach(Vertex v in expected.Keys){
                Assert.IsTrue(expected[v]);
            }
        
            Assert.AreEqual(4, sg2.edges.Count);
            Dictionary<Edge, bool> expectedEdges = new Dictionary<Edge, bool>();
            expectedEdges.Add(new Edge
            {
                V1 = sg2.vertices.Find(vertex => vertex.Name == "Stacja 1").Id,
                V2 = sg2.vertices.Find(vertex => vertex.Name == "Posterunek 1").Id
            }, false);
            expectedEdges.Add(new Edge
            {
                V1 = sg2.vertices.Find(vertex => vertex.Name == "Posterunek 1").Id,
                V2 = sg2.vertices.Find(vertex => vertex.Name == "Posterunek 2").Id
            }, false);
            expectedEdges.Add(new Edge
            {
                V1 = sg2.vertices.Find(vertex => vertex.Name == "Posterunek 2").Id,
                V2 = sg2.vertices.Find(vertex => vertex.Name == "Posterunek 3").Id
            }, false);
            expectedEdges.Add(new Edge
            {
                V1 = sg2.vertices.Find(vertex => vertex.Name == "Posterunek 3").Id,
                V2 = sg2.vertices.Find(vertex => vertex.Name == "Lokomotywownia 1").Id
            }, false);
            foreach(Edge e in sg2.edges){
                foreach(Edge ee in expectedEdges.Keys){
                    if(e.V1 == ee.V1 && e.V2 == ee.V2 || e.V1 == ee.V2 && e.V2 == ee.V1){
                        expectedEdges[ee] = true;
                        break;
                    }
                }
            }
            foreach(Edge e in expectedEdges.Keys){
                Assert.IsTrue(expectedEdges[e]);
            }
        
            Assert.AreEqual(3, sg.lines.Count);
            Dictionary<NamedTree, bool> expectedLines = new Dictionary<NamedTree, bool>();
            expectedLines.Add(new NamedTree{Name = "Linia 1", Tree = new List<int>{
                sg2.vertices.Find(vertex => vertex.Name == "Stacja 1").Id,
                sg2.vertices.Find(vertex => vertex.Name == "Posterunek 1").Id,
                sg2.vertices.Find(vertex => vertex.Name == "Posterunek 2").Id,
                sg2.vertices.Find(vertex => vertex.Name == "Posterunek 3").Id
            }}, false);
            expectedLines.Add(new NamedTree{Name = "Linia 3", Tree = new List<int>{
                sg2.vertices.Find(vertex => vertex.Name == "Posterunek 3").Id,
                sg2.vertices.Find(vertex => vertex.Name == "Lokomotywownia 1").Id
            }}, false);
            foreach(NamedTree nt in sg2.lines){
                foreach(NamedTree ent in expectedLines.Keys){
                    if(nt.Name == ent.Name && nt.Tree == ent.Tree){
                        expectedLines[ent] = true;
                        break;
                    }
                }
            }
            foreach(NamedTree nt in expectedLines.Keys){
                Assert.IsTrue(expectedLines[nt]);
            }
        
            Dictionary<NamedSubgraph, bool> expectedSubgraphs = new Dictionary<NamedSubgraph, bool>();
            expectedSubgraphs.Add(new NamedSubgraph{Name = "LCS1", Verices = new List<int>
            {
                sg2.vertices.Find(vertex => vertex.Name == "Stacja 1").Id,
                sg2.vertices.Find(vertex => vertex.Name == "Posterunek 1").Id,
                sg2.vertices.Find(vertex => vertex.Name == "Posterunek 2").Id
            }}, false);
            expectedSubgraphs.Add(new NamedSubgraph{Name = "LCS2", Verices = new List<int>
            {
                sg2.vertices.Find(vertex => vertex.Name == "Posterunek 3").Id
            }}, false);
            expectedSubgraphs.Add(new NamedSubgraph{Name = "LCS3", Verices = new List<int>
            {
                sg2.vertices.Find(vertex => vertex.Name == "Lokomotywownia 1").Id
            }}, false);
            foreach(NamedSubgraph ns in sg2.subgraphs){
                foreach(NamedSubgraph ens in expectedSubgraphs.Keys){
                    if(ns.Name == ens.Name && ns.Verices == ens.Verices){
                        expectedSubgraphs[ens] = true;
                        break;
                    }
                }
            }
            foreach(NamedSubgraph ns in expectedSubgraphs.Keys){
                Assert.IsTrue(expectedSubgraphs[ns]);
            }
        }
        
    }
    
}