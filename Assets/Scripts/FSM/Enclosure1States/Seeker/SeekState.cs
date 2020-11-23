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

        if (agent.IsTargetNotAtCachedPosition() == true)
        {
            // 16% chance to recalculate path
            if (Random.Range(0,6) < 1)
            {
                Debug.Log("Target has moved");
                sm.PopState();
                sm.PushState(agent.idleState);
            }
        }
        agent.Move();
    }

    public override void Exit()
    {

    }
}
