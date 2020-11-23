using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculatePathState : State
{

    // class initialiser sets owner and sensor to given objects
    public CalculatePathState(AgentAStar agent, FSMStateManager sm) : base(agent, sm)
    {

    }

    public override void Enter()
    {
    }

    public override void Execute()
    {
        agent.CalculatePath();
    }

    public override void Exit()
    {
    }
}

