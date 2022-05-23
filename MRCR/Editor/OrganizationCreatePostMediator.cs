using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MRCR.canvasdrawable;
using MRCR.datastructures;
using Size = System.Drawing.Size;

namespace MRCR.Editor;

public class OrganizationCreatePostMediator : OrganizationCreatePostAbstract, ICanvasMediator
{
    public OrganizationCreatePostMediator(World world, ICanvasManager canvasManager) : base(world, canvasManager){}
    public void ButtonPress(UnifiedPoint worldMouseCoords) {}
    
    public void ButtonRelease(UnifiedPoint mouseCoords)
    {
        mouseCoords.Convert(CoordinatesMode.World, _canvasManager.Scale);
        mouseCoords.X = Math.Round(mouseCoords.X);
        mouseCoords.Y = Math.Round(mouseCoords.Y);
        try
        {
            Post p = _world.AddPost((int)mouseCoords.X, (int)mouseCoords.Y, PostType.Post);
        }
        catch (PostsCollisionException)
        {
            MessageBox.Show("2 posterunki nie mogą być w tym samym miejscu", "Błąd dodawania posterunku", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    public void MouseMove(UnifiedPoint worldMouseCoords, MouseEventArgs? _){ }
}