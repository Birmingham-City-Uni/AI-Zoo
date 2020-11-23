using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    AgentAStar owner;

    public IdleState(AgentAStar agent, FSMStateManager sm) : base(agent, sm)
    {

    }

    public override void Enter()
    {
    }

    public override void Execute()
    {
    }

    public override void Exit()
    {

    }
}
