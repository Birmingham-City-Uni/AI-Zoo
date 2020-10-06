using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor.PackageManager;
using UnityEngine;

public class SensorScript : MonoBehaviour
{

    public LayerMask hitMask;
    public enum Type
    {
        Line,
        BoxCheck,
        SphereCheck,
        RayBundle,
        SphereCast,
        BoxCast,
    }

    public Type sensorType = Type.Line;
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

    Transform cachedTransform;
    public bool Hit { get; private set; }
    public RaycastHit info = new RaycastHit();

    // Start is called before the first frame update
    void Start()
    {
        cachedTransform = GetComponent<Transform>();
        arcLength = 1;
        rayResolution = 1;
    }

    // Update is called once per frame
    void Update()
    {
        Scan();
    }

    public bool Scan()
    {
        Hit = false;
        Vector3 dir = cachedTransform.forward;
        switch (sensorType)
        {
            case Type.Line:
                if (Physics.Linecast(cachedTransform.position, cachedTransform.position + dir * raycastLength, out info, hitMask, QueryTriggerInteraction.Ignore))
                {
                    Hit = true;
                    return true;
                }
                break;
            case Type.BoxCheck:
                if (Physics.CheckBox(this.transform.position, boxExtents/2, this.transform.rotation, hitMask, QueryTriggerInteraction.Ignore))
                {
                    Hit = true;
                    return true;
                }
                break;
            case Type.SphereCheck:
                if (Physics.CheckSphere(this.transform.position, spherecastRadius , hitMask, QueryTriggerInteraction.Ignore))
                {
                    Hit = true;
                    return true;
                }
                break;
            case Type.RayBundle:
                float angleInbetween = arcLength / rayResolution;
                dir.y = dir.y - arcLength / 2;
                for (int i = 0; i < rayResolution; i++)
                {
                    if (Physics.Linecast(cachedTransform.position, cachedTransform.position + dir * raycastLength, out info, hitMask, QueryTriggerInteraction.Ignore))
                    {
                        Hit = true;
                        return true;
                    }
                    dir.y += angleInbetween;
                }
                break;
            case Type.SphereCast:
                if (Physics.SphereCast(new Ray(cachedTransform.position, dir), spherecastRadius, out info, raycastLength, hitMask, QueryTriggerInteraction.Ignore))
                {
                    Hit = true;
                    return true;
                }
                if (Physics.CheckSphere(this.transform.position, spherecastRadius, hitMask, QueryTriggerInteraction.Ignore))
                {
                    Hit = true;
                    return true;
                }
                break;
            case Type.BoxCast:
                if (Physics.BoxCast(cachedTransform.position, boxExtents/2, dir, cachedTransform.rotation,raycastLength, hitMask, QueryTriggerInteraction.Ignore))
                {
                    Hit = true;
                    return true;
                }
                if (Physics.CheckBox(this.transform.position, boxExtents / 2, this.transform.rotation, hitMask, QueryTriggerInteraction.Ignore))
                {
                    Hit = true;
                    return true;
                }
                break;
        }
        return false;
    }

    Vector3 VectorRotate(Vector3 currentPos, Vector3 vectorToRotate, float rotationAngle)
    {
        Vector3 direction = vectorToRotate - currentPos;
        direction = Quaternion.Euler(0, rotationAngle, 0) * direction;
        vectorToRotate = direction + currentPos;
        return vectorToRotate;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        if (cachedTransform == null)
        {
            cachedTransform = GetComponent<Transform>();
        }
        Scan();
        if (Hit)
        {
            Gizmos.color = Color.red;
        }
        Gizmos.matrix *= Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
        float length = raycastLength;
        switch (sensorType)
        {
            case Type.Line:
                if (Hit)
                {
                    length = Vector3.Distance(this.transform.position, info.point);
                }
                Gizmos.DrawLine(Vector3.zero, Vector3.forward * length);
                Gizmos.color = Color.green;
                Gizmos.DrawCube(Vector3.forward * length, new Vector3(0.02f, 0.02f, 0.02f));
                break;
            case Type.BoxCheck:
                Gizmos.DrawWireCube(Vector3.zero, boxExtents);
                break;
            case Type.SphereCheck:
                Gizmos.DrawWireSphere(Vector3.zero, spherecastRadius);
                break;
            case Type.RayBundle:
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
            case Type.SphereCast:
                Gizmos.DrawWireSphere(Vector3.zero, spherecastRadius);

                Gizmos.DrawLine(Vector3.up * spherecastRadius, Vector3.up * spherecastRadius + Vector3.forward * length);
                Gizmos.DrawLine(Vector3.down * spherecastRadius, Vector3.down * spherecastRadius + Vector3.forward * length);
                Gizmos.DrawLine(Vector3.left * spherecastRadius, Vector3.left * spherecastRadius + Vector3.forward * length);
                Gizmos.DrawLine(Vector3.right * spherecastRadius, Vector3.right * spherecastRadius + Vector3.forward * length);

                Gizmos.DrawWireSphere(Vector3.zero + Vector3.forward * length, spherecastRadius);
                break;
            case Type.BoxCast:
                Gizmos.DrawWireCube(Vector3.zero, boxExtents);

                Gizmos.DrawLine(new Vector3(-boxExtents.x, boxExtents.y, 0) / 2, new Vector3(-boxExtents.x, boxExtents.y, 0) / 2 + Vector3.forward * length);
                Gizmos.DrawLine(new Vector3(-boxExtents.x, -boxExtents.y, 0) / 2, new Vector3(-boxExtents.x, -boxExtents.y, 0) / 2 + Vector3.forward * length);
                Gizmos.DrawLine(new Vector3(boxExtents.x, boxExtents.y, 0) / 2, new Vector3(boxExtents.x, boxExtents.y, 0) / 2 + Vector3.forward * length);
                Gizmos.DrawLine(new Vector3(boxExtents.x, -boxExtents.y, 0) / 2, new Vector3(boxExtents.x, -boxExtents.y, 0) / 2 + Vector3.forward * length);

                Gizmos.DrawWireCube(Vector3.zero + Vector3.forward * length, boxExtents);
                break;
        }
    }

}
