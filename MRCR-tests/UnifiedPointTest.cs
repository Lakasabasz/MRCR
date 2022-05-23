using MRCR.datastructures;
using NUnit.Framework;

namespace MRCR_tests;

public class UnifiedPointTest
{
    [Test]
    public void TestUnifiedPoint()
    {
        UnifiedPoint point = new UnifiedPoint(1, 2, CoordinatesMode.World);
        Assert.AreEqual(1, point.X);
        Assert.AreEqual(2, point.Y);
        Assert.AreEqual(CoordinatesMode.World, point.Mode);

        point = new UnifiedPoint(0.5, 0.5, CoordinatesMode.Drawing);
        Assert.AreEqual(0.5, point.X);
        Assert.AreEqual(0.5, point.Y);
        Assert.AreEqual(CoordinatesMode.Drawing, point.Mode);
    }
    
    [Test]
    public void TestUnifiedPointToWorld()
    {
        UnifiedPoint point = new UnifiedPoint(1, 2, CoordinatesMode.World);
        point.Convert(CoordinatesMode.Drawing, 20);
        Assert.AreEqual(20, point.X);
        Assert.AreEqual(40, point.Y);
        Assert.AreEqual(CoordinatesMode.Drawing, point.Mode);
        
        point = new UnifiedPoint(1, 2, CoordinatesMode.Drawing);
        point.Convert(CoordinatesMode.Drawing, 20);
        Assert.AreEqual(1, point.X);
        Assert.AreEqual(2, point.Y);
        Assert.AreEqual(CoordinatesMode.Drawing, point.Mode);

        point = new UnifiedPoint(20, 40, CoordinatesMode.Drawing);
        point.Convert(CoordinatesMode.World, 20);
        Assert.AreEqual(1, point.X);
        Assert.AreEqual(2, point.Y);
        Assert.AreEqual(CoordinatesMode.World, point.Mode);

        point = new UnifiedPoint(20, 40, CoordinatesMode.Drawing);
        point.Convert(CoordinatesMode.Drawing, 20);
        Assert.AreEqual(20, point.X);
        Assert.AreEqual(40, point.Y);
        Assert.AreEqual(CoordinatesMode.Drawing, point.Mode);
    }
}