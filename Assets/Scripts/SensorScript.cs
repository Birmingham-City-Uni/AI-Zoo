using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;

public class SensorScript : MonoBehaviour
{

    // Sets layer mask to everything
    public LayerMask hitMask = ~0;

    // Enum of all sensor types
    public enum SensorType
    {
        Line,
        BoxCheck,
        SphereCheck,
        RayBundle,
        SphereCast,
        BoxCast,
    }

    // Default variables
    public SensorType sensorType = SensorType.Line;

    public float raycastLength = 1.0f;

    public Vector3 boxExtents = new Vector3(1.0f, 1.0f, 1.0f);

    public float spherecastRadius = 5.0f;

    public int rayResolution = 1;

    public int arcLength = 1;

    public bool Hit { get; private set; }
    public RaycastHit info;

    // Scan function
    // Parameters:
    // currentPosition: current object position
    // currentRotation: current object rotation
    // direction: current transform forward direction
    //
    // Returns true when enemies hit

    public bool Scan(Vector3 currentPosition, Quaternion currentRotation, Vector3 direction)
    {
        Hit = false;
        switch (sensorType)
        {
            // Casts line in front of agent
            case SensorType.Line:
                if (Physics.Linecast(currentPosition, currentPosition + direction * raycastLength, out info, hitMask, QueryTriggerInteraction.Ignore))
                {
                    Hit = true;
                    return true;
                }
                break;
            // Checks for object within box with size "boxExtents"
            case SensorType.BoxCheck:
                if (Physics.CheckBox(currentPosition, boxExtents / 2, currentRotation, hitMask, QueryTriggerInteraction.Ignore))
                {
                    Hit = true;
                    return true;
                }
                break;
            // Checks for object within sphere with radius "spherecastRadius"
            case SensorType.SphereCheck:
                if (Physics.CheckSphere(currentPosition, spherecastRadius, hitMask, QueryTriggerInteraction.Ignore))
                {
                    Hit = true;
                    return true;
                }
                break;
            // Casts lines with amount "rayResolution", accross arc "arcLength"
            case SensorType.RayBundle:
                float angleInbetween = arcLength / rayResolution;
                direction.y = direction.y - arcLength / 2;
                for (int i = 0; i < rayResolution; i++)
                {
                    if (Physics.Linecast(currentPosition, currentPosition + direction * raycastLength, out info, hitMask, QueryTriggerInteraction.Ignore))
                    {
                        Hit = true;
                        return true;
                    }
                    direction.y += angleInbetween;
                }
                break;
            // Checks along sphere with radius "spherecastRadius" with length of "raycastLength"
            case SensorType.SphereCast:
                if (Physics.SphereCast(new Ray(currentPosition, direction), spherecastRadius, out info, raycastLength, hitMask, QueryTriggerInteraction.Ignore))
                {
                    Hit = true;
                    return true;
                }
                if (Physics.CheckSphere(currentPosition, spherecastRadius, hitMask, QueryTriggerInteraction.Ignore))
                {
                    Hit = true;
                    return true;
                }
                break;
            // Checks along box with extents "boxExtents / 2" with length of "raycastLength"
            case SensorType.BoxCast:
                if (Physics.BoxCast(currentPosition, boxExtents / 2, direction, currentRotation, raycastLength, hitMask, QueryTriggerInteraction.Ignore))
                {
                    Hit = true;
                    return true;
                }
                if (Physics.CheckBox(currentPosition, boxExtents / 2, currentRotation, hitMask, QueryTriggerInteraction.Ignore))
                {
                    Hit = true;
                    return true;
                }
                break;
        }
        return false;
    }

    // Gives the vector in the direction of rotationAngle from the current position
    public Vector3 VectorRotate(Vector3 currentPos, Vector3 vectorToRotate, float rotationAngle)
    {
        // Finds direction vector
        Vector3 direction = vectorToRotate - currentPos;
        // Multiplies direction by angle of rotation
        direction = Quaternion.Euler(0, rotationAngle, 0) * direction;
        // Adds direction to current position
        vectorToRotate = direction + currentPos;
        // Returns rotated vector
        return vectorToRotate;
    }

    void Update()
    {
        // Calls for sensor scan giving current objects position, rotation and forward facing direction
        Scan(this.transform.position, this.transform.rotation, this.transform.forward);
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
        // Checks for hit, changes colour to red
        if (Hit)
        {
            Gizmos.color = Color.red;
        }
        Gizmos.matrix *= Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
        float length = raycastLength;
        switch (sensorType)
        {
            // Draws sensor line from middle of object infront * length
            case SensorScript.SensorType.Line:
                Gizmos.DrawLine(Vector3.zero, Vector3.forward * length);
                break;
            // Draws box around player, from center with extents of "boxExtents"
            case SensorScript.SensorType.BoxCheck:
                Gizmos.DrawWireCube(Vector3.zero, boxExtents);
                break;
            // Draws Sphere around player, from center with radius of "spherecastRadius"
            case SensorScript.SensorType.SphereCheck:
                Gizmos.DrawWireSphere(Vector3.zero, spherecastRadius);
                break;
            // Shows rays coming from object
            case SensorScript.SensorType.RayBundle:
                float angleInbetween = 0;
                float rotation = 0;
                if (rayResolution > 1)
                {
                    angleInbetween = arcLength / rayResolution;
                    rotation = -arcLength / 2;
                }
                for (int i = 0; i < (rayResolution); i++)
                {
                    rotation += angleInbetween;
                    Gizmos.DrawLine(Vector3.zero, VectorRotate(Vector3.zero, Vector3.forward, rotation) * length);
                }
                break;
            case SensorScript.SensorType.SphereCast:
                // Draws starting sphere
                Gizmos.DrawWireSphere(Vector3.zero, spherecastRadius);

                // Draws connecting lines
                Gizmos.DrawLine(Vector3.up * spherecastRadius, Vector3.up * spherecastRadius + Vector3.forward * length);
                Gizmos.DrawLine(Vector3.down * spherecastRadius, Vector3.down * spherecastRadius + Vector3.forward * length);
                Gizmos.DrawLine(Vector3.left * spherecastRadius, Vector3.left * spherecastRadius + Vector3.forward * length);
                Gizmos.DrawLine(Vector3.right * spherecastRadius, Vector3.right * spherecastRadius + Vector3.forward * length);

                // Draws last sphere
                Gizmos.DrawWireSphere(Vector3.zero + Vector3.forward * length, spherecastRadius);
                break;
            case SensorScript.SensorType.BoxCast:
                // Draws starting box
                Gizmos.DrawWireCube(Vector3.zero, boxExtents);

                // Draws connecting lines
                Gizmos.DrawLine(new Vector3(-boxExtents.x, boxExtents.y, 0) / 2, new Vector3(-boxExtents.x, boxExtents.y, 0) / 2 + Vector3.forward * length);
                Gizmos.DrawLine(new Vector3(-boxExtents.x, -boxExtents.y, 0) / 2, new Vector3(-boxExtents.x, -boxExtents.y, 0) / 2 + Vector3.forward * length);
                Gizmos.DrawLine(new Vector3(boxExtents.x, boxExtents.y, 0) / 2, new Vector3(boxExtents.x, boxExtents.y, 0) / 2 + Vector3.forward * length);
                Gizmos.DrawLine(new Vector3(boxExtents.x, -boxExtents.y, 0) / 2, new Vector3(boxExtents.x, -boxExtents.y, 0) / 2 + Vector3.forward * length);

                // Draws last box
                Gizmos.DrawWireCube(Vector3.zero + Vector3.forward * length, boxExtents);
                break;
        }
    }
}
