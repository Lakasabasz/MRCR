using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MRCR.datastructures;
using NUnit.Framework;

namespace MRCR_tests;

public class NeighbourMatrixTests
{
    [Test]
    public void TestNeighbourMatrix()
    {
        NeighbourMatrix matrix = new NeighbourMatrix();
        Post[] posts = new Post[]
        {
            new Post(PostType.Combined, new Point(0, 0)),
            new Post(PostType.Combined, new Point(1, 0)),
            new Post(PostType.Combined, new Point(1, 1)),
            new Post(PostType.Combined, new Point(0, 1))
        };
        posts[0].SetName("AA");
        posts[1].SetName("XA");
        posts[2].SetName("XX");
        posts[3].SetName("AX");
        foreach (var post in posts) matrix.AddPost(post);
        Assert.AreEqual(4,matrix.GetPostsCount());
        Assert.AreEqual(0, matrix.GetTrailsList().Count);
        
        matrix[posts[0], posts[1]] = new Trail(posts[0], posts[1]);
        Assert.Throws<ArgumentException>(delegate { matrix[posts[1], posts[2]] = new Trail(posts[0], posts[1]); });
        Assert.IsNull(matrix[posts[0], posts[3]]);
        Assert.AreEqual(matrix[posts[0], posts[1]], matrix[posts[1], posts[0]]);
    }

    [Test]
    public void TestNeighbourMatrix2()
    {
        List<Post> posts = new List<Post>
        {
            new (PostType.Combined, new Point(0, 0)),
            new (PostType.Combined, new Point(1, 0)),
            new (PostType.Combined, new Point(1, 1)),
            new (PostType.Combined, new Point(0, 1))
        };
        posts[0].SetName("AA");
        posts[1].SetName("XA");
        posts[2].SetName("XX");
        posts[3].SetName("AX");
        NeighbourMatrix matrix = new NeighbourMatrix(posts);
        matrix[posts[0], posts[1]] = new Trail(posts[0], posts[1]);
        matrix[posts[1], posts[2]] = new Trail(posts[1], posts[2]);
        matrix[posts[2], posts[3]] = new Trail(posts[2], posts[3]);
        matrix[posts[3], posts[0]] = new Trail(posts[3], posts[0]);
        var trails = matrix.GetTrailsList();
        Assert.AreEqual(4, trails.Count);
        var neighbours = matrix[posts[0]];
        Assert.AreEqual(3, neighbours.Count);
        Assert.IsNotNull(neighbours[posts[1]]);
        Assert.IsNull(neighbours[posts[2]]);
        Assert.IsNotNull(neighbours[posts[3]]);
        Assert.IsTrue(matrix.VerifiConsistency());
    }

    [Test]
    public void TestNeighbourMatrix3()
    {
        List<Post> posts = new List<Post>
        {
            new(PostType.Combined, new Point(0, 0)),
            new(PostType.Combined, new Point(1, 0)),
            new(PostType.Combined, new Point(1, 1)),
            new(PostType.Combined, new Point(0, 1)),
            new(PostType.Combined, new Point(2, 0)),
            new(PostType.Combined, new Point(2, 1)),
            new(PostType.Combined, new Point(2, 2))
        };
        posts[0].SetName("AA");
        posts[1].SetName("MA");
        posts[2].SetName("MM");
        posts[3].SetName("AM");
        posts[4].SetName("XA");
        posts[5].SetName("XM");
        posts[6].SetName("XX");
        NeighbourMatrix matrix = new NeighbourMatrix(posts);
        matrix[posts[0], posts[1]] = new Trail(posts[0], posts[1]);
        matrix[posts[1], posts[2]] = new Trail(posts[1], posts[2]);
        matrix[posts[2], posts[3]] = new Trail(posts[2], posts[3]);
        matrix[posts[3], posts[0]] = new Trail(posts[3], posts[0]);
        matrix[posts[4], posts[5]] = new Trail(posts[4], posts[5]);
        matrix[posts[5], posts[6]] = new Trail(posts[5], posts[6]);
        matrix[posts[6], posts[4]] = new Trail(posts[6], posts[4]);
        Assert.IsFalse(matrix.VerifiConsistency());
        Assert.IsTrue(matrix.VerifiConsistency(new List<Post>{ posts[0], posts[1], posts[2], posts[3] }));
        Assert.IsTrue(matrix.VerifiConsistency(new List<Post>{ posts[4], posts[5], posts[6] }));
        NeighbourMatrix matrix2 = matrix.GetSubgraph(new List<Post>{posts[0], posts[1], posts[3], posts[4], posts[6]});
        Assert.IsFalse(matrix2.VerifiConsistency());
        Assert.IsNotNull(matrix2[posts[0], posts[1]]);
        Assert.IsNull(matrix2[posts[0], posts[6]]);
        Assert.Throws<IndexOutOfRangeException>(delegate { Trail? t = matrix2[posts[1], posts[2]]; });
        Assert.Throws<IndexOutOfRangeException>(delegate { var t = matrix2[posts[2]]; });
        var trails = matrix2.GetTrailsList();
        Assert.AreEqual(3, trails.Count);
        matrix2[posts[0], posts[6]] = new Trail(posts[0], posts[6]);
        Assert.IsTrue(matrix2.VerifiConsistency());
    }

    [Test]
    public void TestNeighbourMatrix4()
    {
        List<Post> posts = new List<Post>
        {
            new(PostType.Combined, new Point(0, 0)),
            new(PostType.Combined, new Point(1, 0)),
            new(PostType.Combined, new Point(1, 1)),
            new(PostType.Combined, new Point(0, 1)),
            new(PostType.Combined, new Point(2, 0))
        };
        NeighbourMatrix matrix = new NeighbourMatrix(posts);
        matrix[posts[0], posts[1]] = new Trail(posts[0], posts[1]);
        matrix[posts[1], posts[2]] = new Trail(posts[1], posts[2]);
        matrix[posts[2], posts[3]] = new Trail(posts[2], posts[3]);
        matrix[posts[3], posts[0]] = new Trail(posts[3], posts[0]);
        matrix[posts[4], posts[0]] = new Trail(posts[4], posts[0]);
        matrix[posts[0], posts[2]] = new Trail(posts[0], posts[2]);
        matrix[posts[1], posts[3]] = new Trail(posts[1], posts[3]);
        matrix[posts[1], posts[4]] = new Trail(posts[1], posts[4]);
        matrix[posts[2], posts[4]] = new Trail(posts[2], posts[4]);
        matrix[posts[3], posts[4]] = new Trail(posts[3], posts[4]);
        var neighbours = matrix[posts[0]];
        var missing = matrix[posts[0]];
        missing.Remove(posts[1]);
        var invalid = matrix[posts[0]];
        invalid.Remove(posts[1]);
        invalid.Add(new Post("test", 1, 1, 1), null);
        var invalid2 = matrix[posts[0]];
        invalid2[posts[1]] = new Trail(new Post(PostType.Combined, new Point(1, 2)), posts[1]);
        matrix[posts[0]] = null;
        Assert.AreEqual(0, matrix[posts[0]]!.Count(x => x.Value != null));
        // Set missing to posts[0]
        matrix[posts[0]] = missing;
        Assert.AreEqual(1, matrix[posts[0]]!.Count(x => x.Value == null));
        // Set invalid1 to posts[0] (thorws exception)
        Assert.Catch<IndexOutOfRangeException>(delegate { matrix[posts[0]] = invalid; });
        // Set invalid2 to posts[0] (thorws exception)
        Assert.Catch<ArgumentException>(delegate { matrix[posts[0]] = invalid2; });
        // Set neighbours to posts[0]
        matrix[posts[0]] = neighbours;
        Assert.AreEqual(0, matrix[posts[0]]!.Count(x => x.Value == null));

    }
}