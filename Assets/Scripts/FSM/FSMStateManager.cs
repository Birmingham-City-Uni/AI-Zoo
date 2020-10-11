using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class FSMStateManager
{
    State currentState;

    public void ChangeState(State newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }
        currentState = newState;
        newState.Enter();
    }

    // Update is called once per frame
    public void Update()
    {
        if (currentState != null)
        {
            currentState.Execute();
        }
    }
}
