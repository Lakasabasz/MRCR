using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using MRCR.canvasdrawable;
using MRCR.datastructures;
using Point = System.Drawing.Point;
using Rectangle = MRCR.canvasdrawable.Rectangle;
using Size = System.Drawing.Size;

namespace MRCR.Editor;

public class OrganizationSelectObjectMediator: ICanvasMediator
{
    private ICanvasManager _canvas;
    private UnifiedPoint? _start;
    private UnifiedPoint? _end;
    private World _world;
    
    public OrganizationSelectObjectMediator(ICanvasManager canvas, World world)
    {
        _canvas = canvas;
        IDrawableProxy idp =
            new Rectangle(
                new UnifiedPoint(0, 0),
                new UnifiedPoint(0, 0),
                Brushes.LightSkyBlue,
                _canvas.Scale, ScalePolicy.Fixed);
        idp.GetDrawable().Stroke = Brushes.DodgerBlue;
        idp.GetDrawable().StrokeThickness = 1;
        _canvas.GetCategory("objectSelection").Add(new NameableIDrawableProxy(idp, null));
        _canvas.DisableCategory("objectSelection");
        _world = world;
    }
    
    public void ButtonPress(UnifiedPoint worldMouseCoords)
    {
        _start = worldMouseCoords;
        IDrawableProxy obj = _canvas.GetCategory("objectSelection")[0].Item1;
        _canvas.EnableCategory("objectSelection");
        obj.SetPosition(_start, _canvas.Scale);
        obj.SetSize(new UnifiedPoint(0, 0), _canvas.Scale);
        _canvas.UpdateCanvas();
    }

    public void ButtonRelease(UnifiedPoint mouseCoords)
    {
        _start = null;
        _canvas.DisableCategory("objectSelection");
        _canvas.UpdateCanvas();
    }

    public void MouseMove(UnifiedPoint worldMouseCoords, MouseEventArgs? args)
    {
        if (_start == null) return;
        _end = worldMouseCoords;
        IDrawableProxy obj = _canvas.GetCategory("objectSelection")[0].Item1;
        Rectangle rect = (obj as Rectangle)!;
        rect.DrawBetween(_start, _end, _canvas.Scale);
        _canvas.UpdateCanvas();
        var overlappingObjects = _canvas.GetOverlappingObjects(rect, "objects");
        List<Post> posts = new List<Post>();
        foreach (var overlapping in overlappingObjects)
        {
            Post? p = _world.GetPost(overlapping.Item2);
            if(p == null) throw new Exception("Post not found");
            posts.Add(p);
        }
        (_world.SelectionServices[OrganisationObjectType.Post] as ISelectionService<Post>)!.Set(posts);
        if (args == null) return;
        if (args.LeftButton == MouseButtonState.Released && _start != null)
        {
            ButtonRelease(worldMouseCoords);
        }
    }
}