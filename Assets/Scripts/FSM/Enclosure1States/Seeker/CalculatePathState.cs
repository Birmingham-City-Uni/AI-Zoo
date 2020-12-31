using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculatePathState : State
{

    // class initialiser sets owner and sensor to given objects
    public CalculatePathState(AgentAStar agent, FSMStateManager sm, Animator anim) : base(agent, sm, anim)
    {

    }

    public override void Enter()
    {
    }

    public override void Execute()
    {
        // Calls the calculation method in the AgentAStar class
        agent.CalculatePath();
    }

    public override void Exit()
    {
    }
}

