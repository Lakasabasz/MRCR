using System;
using System.Collections.Generic;
using System.Windows;
using MRCR.canvasdrawable;
using MRCR.datastructures;

namespace MRCR.Editor;

public class NameableIDrawableProxy : Tuple<IDrawableProxy, string?>
{
    public NameableIDrawableProxy(IDrawableProxy item1, string? item2) : base(item1, item2) {}
}

class SwitchableCategory<T> : Tuple<List<T>>
{
    public new bool Item2 { get; set; }

    public SwitchableCategory(List<T> item1, bool item2) : base(item1)
    {
        Item2 = item2;
    }
}

public interface ICanvasManager
{
    void AddUiElement(IDrawableProxy element, string category, string? name);
    void RemoveUiElement(string category, string name);
    void ClearCategory(string category);
    void DisableCategory(string category);
    void EnableCategory(string category);
    List<NameableIDrawableProxy> GetCategory(string category);
    void UpdateCanvas();
    UnifiedPoint ToDrawingCoordinates(UnifiedPoint point);
}