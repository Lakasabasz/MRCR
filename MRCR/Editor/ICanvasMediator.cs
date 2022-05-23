using System.Windows.Input;
using MRCR.datastructures;

namespace MRCR.Editor;

public interface ICanvasMediator
{
    public void ButtonPress(UnifiedPoint worldMouseCoords);
    public void ButtonRelease(UnifiedPoint mouseCoords);
    public void MouseMove(UnifiedPoint worldMouseCoords, MouseEventArgs? args = null);
}