using MRCR.datastructures;

namespace MRCR.Editor;

public abstract class OrganizationCreatePostAbstract
{
    protected World _world;
    protected ICanvasManager _canvasManager;
    
    public OrganizationCreatePostAbstract(World world, ICanvasManager canvasManager)
    {
        _world = world;
        _canvasManager = canvasManager;
    }
}