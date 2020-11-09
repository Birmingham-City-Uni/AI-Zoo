using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculatePathState : State
{

    // class initialiser sets owner and sensor to given objects
    public CalculatePathState(Agent agent, FSMStateManager sm) : base(agent, sm)
    {

    }

    public override void Enter()
    {
        Debug.Log("Enter CalculatingPath");
    }

    public override void Execute()
    {
        Debug.Log("Executing CalculatingPath");
        agent.CalculatePath();
    }

    public override void Exit()
    {
        Debug.Log("Exiting CalculatingPath");
    }
}

