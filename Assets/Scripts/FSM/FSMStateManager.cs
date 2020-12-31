using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class FSMStateManager
{
    // Creates stack container
    private Stack stack;

    public void Init(State initialState)
    {
        // Creates new stack
        this.stack = new Stack();
        // Pushes state given upon FSM init
        stack.Push(initialState);
        initialState.Enter();
    }

    // Pops current state off of the stack
    public bool PopState()
    {
        // Checks if FSM contains a state
        if (stack.Count > 0)
        {
            GetCurrentState().Exit();
            stack.Pop();
            return true;
        }
        else
        {
            return false;
        }
    }

    // Push state onto stack
    public bool PushState(State pushedState)
    {
        if (this.stack.Count == 0)
        {
            this.stack.Push(pushedState);
            GetCurrentState().Enter();
            return true;
        }
        // Checks that newly pushed state is not already pushed
        else if (this.stack.Peek() != pushedState)
        {
            this.stack.Push(pushedState);
            GetCurrentState().Enter();
            return true;
        }
        else
        {
            return false;
        }
    }

    // Returns the current state
    public State GetCurrentState()
    {
        if (this.stack.Count > 0)
        {
            return (State)this.stack.Peek();
        }
        else
        {
            return null;
        }
    }

    // Update is called once per frame
    public void Update()
    {
        // Calls the execute of the current state if a state exists on the stack
        if (GetCurrentState() != null)
        {
            GetCurrentState().Execute();
        }
    }
}
