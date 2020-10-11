using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    // Methods to be defined on use
    public abstract void Enter();
    public abstract void Execute();
    public abstract void Exit();
}