using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanGizmos : MonoBehaviour
{
    Agent agent;
    SensorScript sensor = new SensorScript();

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
        if (Application.isPlaying == false) return;
        Gizmos.color = Color.white;
        sensor.Scan(this.transform.position, this.transform.rotation, this.transform.forward);
        if (sensor.Hit)
        {
            Gizmos.color = Color.red;
        }
        Gizmos.matrix *= Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
        float length = sensor.raycastLength;
        switch (sensor.sensorType)
        {
            case SensorScript.SensorType.Line:
                if (sensor.Hit)
                {
                    length = Vector3.Distance(this.transform.position, sensor.info.point);
                }
                Gizmos.DrawLine(Vector3.zero, Vector3.forward * length);
                Gizmos.color = Color.green;
                Gizmos.DrawCube(Vector3.forward * length, new Vector3(0.02f, 0.02f, 0.02f));
                break;
            case SensorScript.SensorType.BoxCheck:
                Gizmos.DrawWireCube(Vector3.zero, sensor.boxExtents);
                break;
            case SensorScript.SensorType.SphereCheck:
                Gizmos.DrawWireSphere(Vector3.zero, sensor.spherecastRadius);
                break;
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
                Gizmos.DrawWireSphere(Vector3.zero, sensor.spherecastRadius);

                Gizmos.DrawLine(Vector3.up * sensor.spherecastRadius, Vector3.up * sensor.spherecastRadius + Vector3.forward * length);
                Gizmos.DrawLine(Vector3.down * sensor.spherecastRadius, Vector3.down * sensor.spherecastRadius + Vector3.forward * length);
                Gizmos.DrawLine(Vector3.left * sensor.spherecastRadius, Vector3.left * sensor.spherecastRadius + Vector3.forward * length);
                Gizmos.DrawLine(Vector3.right * sensor.spherecastRadius, Vector3.right * sensor.spherecastRadius + Vector3.forward * length);

                Gizmos.DrawWireSphere(Vector3.zero + Vector3.forward * length, sensor.spherecastRadius);
                break;
            case SensorScript.SensorType.BoxCast:
                Gizmos.DrawWireCube(Vector3.zero, sensor.boxExtents);

                Gizmos.DrawLine(new Vector3(-sensor.boxExtents.x, sensor.boxExtents.y, 0) / 2, new Vector3(-sensor.boxExtents.x, sensor.boxExtents.y, 0) / 2 + Vector3.forward * length);
                Gizmos.DrawLine(new Vector3(-sensor.boxExtents.x, -sensor.boxExtents.y, 0) / 2, new Vector3(-sensor.boxExtents.x, -sensor.boxExtents.y, 0) / 2 + Vector3.forward * length);
                Gizmos.DrawLine(new Vector3(sensor.boxExtents.x, sensor.boxExtents.y, 0) / 2, new Vector3(sensor.boxExtents.x, sensor.boxExtents.y, 0) / 2 + Vector3.forward * length);
                Gizmos.DrawLine(new Vector3(sensor.boxExtents.x, -sensor.boxExtents.y, 0) / 2, new Vector3(sensor.boxExtents.x, -sensor.boxExtents.y, 0) / 2 + Vector3.forward * length);

                Gizmos.DrawWireCube(Vector3.zero + Vector3.forward * length, sensor.boxExtents);
                break;
        }
    }
}
