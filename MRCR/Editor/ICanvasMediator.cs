using MRCR.datastructures;

namespace MRCR.Editor;

public interface ICanvasMediator
{
    public void ButtonPress(UnifiedPoint worldMouseCoords);
    public void ButtonRelease(UnifiedPoint worldMouseCoords);
}