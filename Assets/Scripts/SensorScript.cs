using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UIElements;

public class SensorScript : MonoBehaviour
{

    // Sets layer mask to everything
    public LayerMask hitMask = ~0;

    public enum SensorType
    {
        Line,
        BoxCheck,
        SphereCheck,
        RayBundle,
        SphereCast,
        BoxCast,
    }

    public SensorType sensorType = SensorType.Line;

    public float raycastLength = 1.0f;

    public Vector3 boxExtents = new Vector3(1.0f, 1.0f, 1.0f);

    public float spherecastRadius = 5.0f;

    public int rayResolution;

    public int arcLength;

    public bool Hit { get; private set; }
    public RaycastHit info;

    // Start is called before the first frame update
    void Start()
    {
        arcLength = 1;
        rayResolution = 1;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool Scan(Vector3 currentPosition, Quaternion currentRotation, Vector3 direction)
    {
        Hit = false;
        switch (sensorType)
        {
            case SensorType.Line:
                if (Physics.Linecast(currentPosition, currentPosition + direction * raycastLength, out info, hitMask, QueryTriggerInteraction.Ignore))
                {
                    Hit = true;
                    return true;
                }
                break;
            case SensorType.BoxCheck:
                if (Physics.CheckBox(currentPosition, boxExtents / 2, currentRotation, hitMask, QueryTriggerInteraction.Ignore))
                {
                    Hit = true;
                    return true;
                }
                break;
            case SensorType.SphereCheck:
                if (Physics.CheckSphere(currentPosition, spherecastRadius, hitMask, QueryTriggerInteraction.Ignore))
                {
                    Hit = true;
                    return true;
                }
                break;
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

    public Vector3 VectorRotate(Vector3 currentPos, Vector3 vectorToRotate, float rotationAngle)
    {
        Vector3 direction = vectorToRotate - currentPos;
        direction = Quaternion.Euler(0, rotationAngle, 0) * direction;
        vectorToRotate = direction + currentPos;
        return vectorToRotate;
    }
}
