using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class FSMStateManager
{
    State currentState;

    // Changes state to given state
    public void ChangeState(State newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }
        // Enters state given
        currentState = newState;
        newState.Enter();
    }

    // Update is called once per frame
    public void Update()
    {
        if (currentState != null)
        {
            // Calls current state
            currentState.Execute();
        }
    }
}
