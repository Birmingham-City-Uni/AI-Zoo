using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekState : State
{

    // class initialiser sets owner and sensor to given objects
    public SeekState(Agent agent, FSMStateManager sm) : base(agent, sm)
    {

    }

    private IdleState idleState;

    public override void Enter()
    {
        Debug.Log("Entering Seek");
    }

    public override void Execute()
    {
        Debug.Log("Executing Seek");

        if (agent.IsTargetNotAtCachedPosition() == true)
        {
            Debug.Log("Target has moved");
            sm.PopState();
            sm.PushState(idleState);
        }
        agent.Move();
    }

    public override void Exit()
    {
        Debug.Log("Exiting Seek");
    }
}
