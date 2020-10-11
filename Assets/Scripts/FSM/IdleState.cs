using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    Agent owner;

    public IdleState(Agent owner)
    {
        this.owner = owner;
    }

    public override void Enter()
    {
        Debug.Log("Entering Idle");
    }

    public override void Execute()
    {
        Debug.Log("Executing Idle");
    }

    public override void Exit()
    {
        Debug.Log("Exiting Idle");
    }
}
