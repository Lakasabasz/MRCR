using System;
using System.Collections.Generic;

namespace MRCR.Editor;

public class CanvasMediator
{
    private Dictionary<EditorMode, Dictionary<ActionType, ICanvasMediator>> _mediators;

    public CanvasMediator()
    {
        _mediators = new Dictionary<EditorMode, Dictionary<ActionType, ICanvasMediator>>();
    }

    public void Register(EditorMode mode, ActionType actionType, ICanvasMediator mediator)
    {
        if (!_mediators.ContainsKey(mode))
        {
            _mediators[mode] = new Dictionary<ActionType, ICanvasMediator>();
        }
        
        _mediators[mode][actionType] = mediator;
    }
    
    public ICanvasMediator Mediate(EditorMode mode, ActionType actionType)
    {
        if (!_mediators.ContainsKey(mode))
        {
            throw new Exception("No mediator registered for mode " + mode);
        }
        if(!_mediators[mode].ContainsKey(actionType))
        {
            throw new Exception("No mediator registered for action " + actionType);
        }
        return _mediators[mode][actionType];
    }
}