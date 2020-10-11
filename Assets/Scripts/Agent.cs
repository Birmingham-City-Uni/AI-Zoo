using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;


public class Agent : MonoBehaviour
{
    // Creates instances of sensor and FSM 
    FSMStateManager sm = new FSMStateManager();
    SensorScript sensor = new SensorScript();

    // States used by agent
    private enum State
    {
        Idle,
        Sensing,
    }

    State currentState = State.Idle;

    // Default in-editor settings for sense script
    public SensorScript.SensorType sensorType = SensorScript.SensorType.Line;
    public float raycastLength = 1.0f;
    [Header("BoxExtent Settings")]
    public Vector3 boxExtents = new Vector3(1.0f, 1.0f, 1.0f);
    [Header("SphereCast Settings")]
    public float spherecastRadius = 5.0f;
    [Header("RayBundle Settings")]
    [Range(1, 20)]
    public int rayResolution;
    [Range(1, 360)]
    public int arcLength;


    // Start is called before the first frame update
    void Start()
    {
        // Gives sensor scripts the in-editor variables set above
        sensor.sensorType = this.sensorType;
        sensor.boxExtents = this.boxExtents;
        sensor.raycastLength = this.raycastLength;
        sensor.spherecastRadius = this.spherecastRadius;
        sensor.rayResolution = this.rayResolution;
        sensor.arcLength = this.arcLength;

        // Sets state to seek
        sm.ChangeState(new SeekState(this, sensor));
    }

    // Update is called once per frame
    void Update()
    {
        // Calls state update
        sm.Update();
        // Calls state selector
        StateSelector();
    }

    void StateSelector()
    {
        // Checks for sensor hit and if state is not currently idle
        if (sensor.Hit == true && currentState != State.Idle)
        {
            sm.ChangeState(new IdleState(this));
            // Changes enum to current state
            currentState = State.Idle;
        }

        if (sensor.Hit == false && currentState != State.Sensing)
        {
            sm.ChangeState(new SeekState(this, sensor));
            currentState = State.Sensing;
        }
    }
}
