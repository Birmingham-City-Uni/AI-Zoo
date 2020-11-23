using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{

    protected AgentAStar agent;
    protected FSMStateManager sm;

    protected State(AgentAStar _agent, FSMStateManager _sm)
    {
        agent = _agent;
        sm = _sm;
    }

    // Methods to be defined on use
    public abstract void Enter();
    public abstract void Execute();
    public abstract void Exit();
}