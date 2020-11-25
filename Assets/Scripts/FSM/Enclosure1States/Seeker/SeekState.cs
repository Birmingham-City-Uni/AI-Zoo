using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekState : State
{

    // class initialiser sets owner and sensor to given objects
    public SeekState(AgentAStar agent, FSMStateManager sm) : base(agent, sm)
    {

    }

    public override void Enter()
    {

    }

    public override void Execute()
    {
        agent.Move();
    }

    public override void Exit()
    {

    }
}
