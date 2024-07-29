using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState
{
    protected BaseStateMachine StateMachine;

    public BaseState(BaseStateMachine stateMachine)
    {
        StateMachine = stateMachine;
    }
    
    public virtual void EnterState() {}
    public virtual void UpdateState() {}
    public virtual void ExitState() {}
}
