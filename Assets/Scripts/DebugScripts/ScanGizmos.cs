using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanGizmos : MonoBehaviour
{
    Agent agent;
    SensorScript sensor = new SensorScript();

    // Initalisation of variables to those given in editor to agent script
    public void Start()
    {
        agent = GetComponent<Agent>();
        sensor.sensorType = agent.sensorType;
        sensor.boxExtents = agent.boxExtents;
        sensor.raycastLength = agent.raycastLength;
        sensor.spherecastRadius = agent.spherecastRadius;
        sensor.rayResolution = agent.rayResolution;
        sensor.arcLength = agent.arcLength;
    }

    void OnDrawGizmos()
    {
        // Checks whether application is running
        // Removes error messages when not running
        if (Application.isPlaying == false)
        {
            return;
        }
        Gizmos.color = Color.white;
        // Calls for sensor scan giving current objects position, rotation and forward facing direction
        sensor.Scan(this.transform.position, this.transform.rotation, this.transform.forward);
        // Checks for hit, changes colour to red
        if (sensor.Hit)
        {
            Gizmos.color = Color.red;
        }
        Gizmos.matrix *= Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
        float length = sensor.raycastLength;
        switch (sensor.sensorType)
        {
            // Draws sensor line from middle of object infront * length
            case SensorScript.SensorType.Line:
                Gizmos.DrawLine(Vector3.zero, Vector3.forward * length);
                break;
            // Draws box around player, from center with extents of "boxExtents"
            case SensorScript.SensorType.BoxCheck:
                Gizmos.DrawWireCube(Vector3.zero, sensor.boxExtents);
                break;
            // Draws Sphere around player, from center with radius of "spherecastRadius"
            case SensorScript.SensorType.SphereCheck:
                Gizmos.DrawWireSphere(Vector3.zero, sensor.spherecastRadius);
                break;
            // Shows rays coming from object
            case SensorScript.SensorType.RayBundle:
                float angleInbetween = 0;
                float rotation = 0;
                if (sensor.rayResolution > 1)
                {
                    angleInbetween = sensor.arcLength / sensor.rayResolution;
                    rotation = -sensor.arcLength / 2;
                }
                for (int i = 0; i < (sensor.rayResolution); i++)
                {
                    rotation += angleInbetween;
                    Gizmos.DrawLine(Vector3.zero, sensor.VectorRotate(Vector3.zero, Vector3.forward, rotation) * length);
                }
                break;
            case SensorScript.SensorType.SphereCast:
                // Draws starting sphere
                Gizmos.DrawWireSphere(Vector3.zero, sensor.spherecastRadius);

                // Draws connecting lines
                Gizmos.DrawLine(Vector3.up * sensor.spherecastRadius, Vector3.up * sensor.spherecastRadius + Vector3.forward * length);
                Gizmos.DrawLine(Vector3.down * sensor.spherecastRadius, Vector3.down * sensor.spherecastRadius + Vector3.forward * length);
                Gizmos.DrawLine(Vector3.left * sensor.spherecastRadius, Vector3.left * sensor.spherecastRadius + Vector3.forward * length);
                Gizmos.DrawLine(Vector3.right * sensor.spherecastRadius, Vector3.right * sensor.spherecastRadius + Vector3.forward * length);

                // Draws last sphere
                Gizmos.DrawWireSphere(Vector3.zero + Vector3.forward * length, sensor.spherecastRadius);
                break;
            case SensorScript.SensorType.BoxCast:
                // Draws starting box
                Gizmos.DrawWireCube(Vector3.zero, sensor.boxExtents);

                // Draws connecting lines
                Gizmos.DrawLine(new Vector3(-sensor.boxExtents.x, sensor.boxExtents.y, 0) / 2, new Vector3(-sensor.boxExtents.x, sensor.boxExtents.y, 0) / 2 + Vector3.forward * length);
                Gizmos.DrawLine(new Vector3(-sensor.boxExtents.x, -sensor.boxExtents.y, 0) / 2, new Vector3(-sensor.boxExtents.x, -sensor.boxExtents.y, 0) / 2 + Vector3.forward * length);
                Gizmos.DrawLine(new Vector3(sensor.boxExtents.x, sensor.boxExtents.y, 0) / 2, new Vector3(sensor.boxExtents.x, sensor.boxExtents.y, 0) / 2 + Vector3.forward * length);
                Gizmos.DrawLine(new Vector3(sensor.boxExtents.x, -sensor.boxExtents.y, 0) / 2, new Vector3(sensor.boxExtents.x, -sensor.boxExtents.y, 0) / 2 + Vector3.forward * length);

                // Draws last box
                Gizmos.DrawWireCube(Vector3.zero + Vector3.forward * length, sensor.boxExtents);
                break;
        }
    }
}
