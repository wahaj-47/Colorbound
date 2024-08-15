using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStateMachine : MonoBehaviour
{
    private BaseState _currentState;
    public BaseState CurrentState => _currentState;

    public virtual void OnEnable()
    {
        _currentState = GetInitialState();
        _currentState?.EnterState();
    }

    public virtual void OnDisable()
    {
        _currentState?.ExitState();
    }

    public virtual void Start()
    {
        _currentState = GetInitialState();
        _currentState?.EnterState();
    }

    public virtual void Update()
    {
        _currentState?.UpdateState();
    }

    public virtual void SwitchState(BaseState state)
    {
        if (state != null)
        {
            _currentState.ExitState();
            _currentState = state;
            _currentState.EnterState();
        }
    }
    public virtual BaseState GetInitialState()
    {
        return null;
    }

    public virtual void OnDestroy()
    {
        _currentState = null;
    }
}
