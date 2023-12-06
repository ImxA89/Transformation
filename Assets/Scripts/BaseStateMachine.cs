using System;
using System.Collections.Generic;

public class BaseStateMachine
{
    private Dictionary<Type, IBaseState> _states;
    private Base _base;
    private IBaseState _currentState;

    public BaseStateMachine(Base currentBase)
    {
        _base = currentBase;
        InitiateStates();
        SetStateByDefault();
    }

    public void Run()
    {
        _currentState.Run();
    }

    private void InitiateStates()
    {
        _states = new Dictionary<Type, IBaseState>();

        _states[typeof(BaseSpawnWorkersState)] = new BaseSpawnWorkersState(_base);
        _states[typeof(BasePrepareCreatNewBaseState)] = new BasePrepareCreatNewBaseState(_base);
    }

    public void SetWorkerBuildState()
    {
        if (_currentState != GetState<BaseSpawnWorkersState>())
            SetState(GetState<BaseSpawnWorkersState>());
    }

    public void SetNewBaseBiuldState()
    {
        if (_currentState != GetState<BasePrepareCreatNewBaseState>())
            SetState(GetState<BasePrepareCreatNewBaseState>());
    }

    private void SetStateByDefault()
    {
        SetWorkerBuildState();
    }

    private void SetState(IBaseState baseState)
    {
        _currentState = baseState;
        _currentState.Enter();
    }

    private IBaseState GetState<T>() where T : IBaseState
    {
        Type type = typeof(T);

        return _states[type];
    }
}

