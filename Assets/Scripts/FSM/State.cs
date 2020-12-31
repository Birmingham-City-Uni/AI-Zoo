using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base class for State
public abstract class State
{
	
    protected AgentAStar agent;
    protected FSMStateManager sm;
    protected Animator anim;

    protected State(AgentAStar _agent, FSMStateManager _sm, Animator _anim)
    {
        agent = _agent;
        sm = _sm;
        anim = _anim;
    }

    // Methods to be defined on use
    public abstract void Enter();
    public abstract void Execute();
    public abstract void Exit();
}