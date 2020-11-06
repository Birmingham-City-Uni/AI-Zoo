using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class FSMStateManager
{

    private Stack stack;

    public void Init(State initialState)
    {
        this.stack = new Stack();
        stack.Push(initialState);
        initialState.Enter();
    }

    public bool PopState()
    {
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

    public bool PushState(State pushedState)
    {
        if (this.stack.Count == 0)
        {
            this.stack.Push(pushedState);
            GetCurrentState().Enter();
            return true;
        }
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

    public State GetCurrentState()
    {
        if (this.stack.Count > 0)
        {
            return (State) this.stack.Peek();
        }
        else
        {
            return null;
        }
    }

    // Update is called once per frame
    public void Update()
    {
        if (GetCurrentState() != null)
        {
            // Calls current state
            GetCurrentState().Execute();
        }
    }
}
