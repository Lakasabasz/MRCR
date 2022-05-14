using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MRCR.canvasdrawable;
using MRCR.datastructures;
using Size = System.Drawing.Size;

namespace MRCR.Editor;

public class OrganizationCreatePostMediator : OrganizationCreatePostAbstract, ICanvasMediator
{
    public OrganizationCreatePostMediator(World world, ICanvasManager canvasManager) : base(world, canvasManager){}
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
            Brush br = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/MRCR;component/icons/add-train-post.png")));
            _canvasManager.AddUiElement(new Rectangle(new Size(30, 30), drawingCoords.Move(-15, -15).ToDrawingPoint(), br), "objects", p.GetName());
            _canvasManager.UpdateCanvas();
        }
        catch (PostsCollisionException)
        {
            MessageBox.Show("2 posterunki nie mogą być w tym samym miejscu", "Błąd dodawania posterunku", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}