using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MRCR.canvasdrawable;
using MRCR.datastructures;
using Size = System.Drawing.Size;

namespace MRCR.Editor;

public class OrganizationCreatePostMediator : ICanvasMediator
{
    private World _world;
    private ICanvasManager _canvas;
    
    public OrganizationCreatePostMediator(World world, ICanvasManager canvas)
    {
        _world = world;
        _canvas = canvas;
    }
    
    public void ButtonPress(UnifiedPoint worldMouseCoords)
    {
        throw new System.NotImplementedException();
    }
    
    public void ButtonRelease(UnifiedPoint worldMouseCoords)
    {
        var mouseCords = worldMouseCoords.ToDrawingPoint();
        try
        {
            Post p = _world.AddPost(mouseCords.X, mouseCords.Y, PostType.Post);
            var drawingCoords = _canvas.ToDrawingCoordinates(worldMouseCoords);
            _canvas.AddUiElement(new Ellipse(new Size(10, 10), drawingCoords.Move(-5/2, -5/2).ToDrawingPoint(), Brushes.Red), "objects", p.GetName());
            _canvas.UpdateCanvas();
        }
        catch (PostsCollisionException)
        {
            MessageBox.Show("2 posterunki nie mogą być w tym samym miejscu", "Błąd dodawania posterunku", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}