using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekState : State
{

    // class initialiser sets owner and sensor to given objects
    public SeekState(AgentAStar agent, FSMStateManager sm, Animator anim) : base(agent, sm, anim)
    {

    }

    public override void Enter()
    {
        // Reduces animation speed
        anim.speed = 0.5f;
    }

    public override void Execute()
    {
        // Calls move function
        agent.Move();
        // Sets animation
        anim.Play("Fly Inplace");
    }

    public override void Exit()
    {

    }
}
