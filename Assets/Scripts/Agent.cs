using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    FSMStateManager sm = new FSMStateManager();
    SensorScript sensor = new SensorScript();

    bool stateChange = false;

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
        sensor.sensorType = this.sensorType;
        sensor.boxExtents = this.boxExtents;
        sensor.raycastLength = this.raycastLength;
        sensor.spherecastRadius = this.spherecastRadius;
        sensor.rayResolution = this.rayResolution;
        sensor.arcLength = this.arcLength;

        sm.ChangeState(new SeekState(this, sensor));
    }

    // Update is called once per frame
    void Update()
    {
        sm.Update();
        if (sensor.Hit == true && stateChange == true)
        {
            sm.ChangeState(new IdleState(this));
            stateChange = !stateChange;
        }
        if (sensor.Hit == false && stateChange == false)
        {
            sm.ChangeState(new SeekState(this, sensor));
            stateChange = !stateChange;
        }
    }
}
