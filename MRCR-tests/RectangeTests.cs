using System.Threading;
using System.Windows.Controls;
using System.Windows.Media;
using MRCR.canvasdrawable;
using MRCR.datastructures;
using NUnit.Framework;

namespace MRCR_tests;

public class RectangeTests
{
    [Test, Apartment(ApartmentState.STA)]
    public void RectangleResize()
    {
        Rectangle rectangle = new Rectangle(
            new UnifiedPoint(0, 0),
            new UnifiedPoint(0, 0),
            Brushes.Black, 10, ScalePolicy.Fixed);
        rectangle.SetSize(new UnifiedPoint(20, 10, CoordinatesMode.Drawing), 10);
        var drawable = rectangle.GetDrawable();
        Assert.AreEqual(20, drawable.Width);
        Assert.AreEqual(10, drawable.Height);
        
        rectangle.SetSize(new UnifiedPoint(20, 10, CoordinatesMode.World), 10);
        drawable = rectangle.GetDrawable();
        Assert.AreEqual(200, drawable.Width);
        Assert.AreEqual(100, drawable.Height);
        
        rectangle = new Rectangle(
            new UnifiedPoint(0, 0),
            new UnifiedPoint(0, 0),
            Brushes.Black, 10, ScalePolicy.Both);
        rectangle.SetSize(new UnifiedPoint(20, 10, CoordinatesMode.Drawing), 10);
        drawable = rectangle.GetDrawable();
        Assert.AreEqual(20, drawable.Width);
        Assert.AreEqual(10, drawable.Height);
        
        rectangle.SetSize(new UnifiedPoint(20, 10, CoordinatesMode.World), 10);
        drawable = rectangle.GetDrawable();
        Assert.AreEqual(200, drawable.Width);
        Assert.AreEqual(100, drawable.Height);
    }

    [Test, Apartment(ApartmentState.STA)]
    public void DrawBetweenTestStraight()
    {
        Rectangle rectangle = new Rectangle(
            new UnifiedPoint(0, 0),
            new UnifiedPoint(0, 0),
            Brushes.Black, 10, ScalePolicy.Fixed);
        rectangle.DrawBetween(
            new UnifiedPoint(1, 1, CoordinatesMode.World),
            new UnifiedPoint(2, 3, CoordinatesMode.World), 
            10);
        var drawable = rectangle.GetDrawable();
        Assert.AreEqual(10, Canvas.GetTop(drawable));
        Assert.AreEqual(10, Canvas.GetLeft(drawable));
        Assert.AreEqual(10, drawable.Width);
        Assert.AreEqual(20, drawable.Height);
    }
    
    [Test, Apartment(ApartmentState.STA)]
    public void DrawBetweenTestDiagonal()
    {
        Rectangle rectangle = new Rectangle(
            new UnifiedPoint(0, 0),
            new UnifiedPoint(0, 0),
            Brushes.Black, 10, ScalePolicy.Fixed);
        rectangle.DrawBetween(
            new UnifiedPoint(3, 2, CoordinatesMode.World),
            new UnifiedPoint(1, 4, CoordinatesMode.World), 
            10);
        var drawable = rectangle.GetDrawable();
        Assert.AreEqual(20, Canvas.GetTop(drawable));
        Assert.AreEqual(10, Canvas.GetLeft(drawable));
        Assert.AreEqual(20, drawable.Width);
        Assert.AreEqual(20, drawable.Height);
    }

    [Test, Apartment(ApartmentState.STA)]
    public void IsInsideTest()
    {
        Rectangle rectangle = new Rectangle(
            new UnifiedPoint(1, 1, CoordinatesMode.World),
            new UnifiedPoint(1, 1, CoordinatesMode.World),
            Brushes.Black, 10, ScalePolicy.Fixed);
        Assert.IsFalse(rectangle.IsInside(new UnifiedPoint(0, 0, CoordinatesMode.World)));
        Assert.IsFalse(rectangle.IsInside(new UnifiedPoint(0, 1, CoordinatesMode.World)));
        Assert.IsFalse(rectangle.IsInside(new UnifiedPoint(1, 0, CoordinatesMode.World)));
        Assert.IsTrue(rectangle.IsInside(new UnifiedPoint(1, 1, CoordinatesMode.World)));
        Assert.IsTrue(rectangle.IsInside(new UnifiedPoint(1.5, 1.5, CoordinatesMode.World)));
        Assert.IsTrue(rectangle.IsInside(new UnifiedPoint(2, 2, CoordinatesMode.World)));
        Assert.IsFalse(rectangle.IsInside(new UnifiedPoint(2, 3, CoordinatesMode.World)));
        Assert.IsFalse(rectangle.IsInside(new UnifiedPoint(3, 2, CoordinatesMode.World)));
        Assert.IsFalse(rectangle.IsInside(new UnifiedPoint(3, 3, CoordinatesMode.World)));
    }
}