using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    AgentAStar owner;

    public IdleState(AgentAStar agent, FSMStateManager sm, Animator anim) : base(agent, sm, anim)
    {

    }

    public override void Enter()
    {
        // Sets anim speed to default
        anim.speed = 1.0f;
    }

    public override void Execute()
    {
        // Plays idle animation
        anim.Play("Idle");
    }

    public override void Exit()
    {

    }
}
