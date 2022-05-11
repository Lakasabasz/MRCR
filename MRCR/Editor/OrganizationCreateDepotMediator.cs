using System.Windows;
using System.Windows.Media;
using MRCR.canvasdrawable;
using MRCR.datastructures;
using Size = System.Drawing.Size;

namespace MRCR.Editor;

public class OrganizationCreateDepotMediator : OrganizationCreatePostAbstract, ICanvasMediator
{
    public OrganizationCreateDepotMediator(World world, ICanvasManager canvas) : base(world, canvas) { }
    
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
            var drawingCoords = _canvasManager.ToDrawingCoordinates(worldMouseCoords);
            _canvasManager.AddUiElement(new Ellipse(new Size(10, 10), drawingCoords.Move(-5/2, -5/2).ToDrawingPoint(), Brushes.LightGreen), "objects", p.GetName());
            _canvasManager.UpdateCanvas();
        }
        catch (PostsCollisionException)
        {
            MessageBox.Show("2 posterunki nie mogą być w tym samym miejscu", "Błąd dodawania posterunku", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}