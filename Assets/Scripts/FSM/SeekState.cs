using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekState : State
{
    Agent owner;
    SensorScript sensor;

    // class initialiser sets owner and sensor to given objects
    public SeekState(Agent owner, SensorScript sensor)
    {
        this.owner = owner;
        this.sensor = sensor;
    }

    public override void Enter()
    {
        Debug.Log("Entering Seek");
    }

    public override void Execute()
    {
        Debug.Log("Executing Seek");
        // Scans whilst seeking
        sensor.Scan(owner.transform.position, owner.transform.rotation, owner.transform.forward);
    }

    public override void Exit()
    {
        Debug.Log("Exiting Seek");
    }
}
