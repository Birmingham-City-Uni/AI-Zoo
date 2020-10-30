using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{

    // Returns cross product of 2 vectors
    Vector3 Cross(Vector3 a, Vector3 b)
    {

        Vector3 crossProd = new Vector3(0, 0, 0);

        crossProd.x = (a.y * b.z) - (a.z * b.y);
        crossProd.y = (a.z * b.x) - (a.x * b.z);
        crossProd.z = (a.x * b.y) - (a.y * b.x);

        return crossProd;
    }

    // Calculate the vector to the seek object
    public float CalculateAngle(GameObject agentPos, Vector3 targetPos)
    {

        // Vector to the seek object
        Vector3 vectorToFindObject = targetPos - agentPos.transform.position;

        // Dot product calculation

        float dot = (agentPos.transform.forward.x * vectorToFindObject.x) + (agentPos.transform.forward.y * vectorToFindObject.y) + (agentPos.transform.forward.z * vectorToFindObject.z);

        // Calculate angle between objects

        float lengthA = Mathf.Sqrt((agentPos.transform.forward.x * agentPos.transform.forward.x) + (agentPos.transform.forward.y * agentPos.transform.forward.y) + (agentPos.transform.forward.z * agentPos.transform.forward.z));
        float lengthB = Mathf.Sqrt((vectorToFindObject.x * vectorToFindObject.x) + (vectorToFindObject.y * vectorToFindObject.y) + (vectorToFindObject.z * vectorToFindObject.z));

        float angle = Mathf.Acos(angle = dot / (lengthA * lengthB)) * 180 / Mathf.PI;

        // Gets cross product

        Vector3 crossProd = Cross(agentPos.transform.forward, vectorToFindObject);

        // Checks if positive

        if (crossProd.y < 0)
        {
            angle *= -1;
        }

        // Forward facing ray
        Debug.DrawRay(agentPos.transform.position, agentPos.transform.forward * CalculateDistance(agentPos, targetPos), Color.green, 2.0f);
        // Ray to finder object
        Debug.DrawRay(agentPos.transform.position, vectorToFindObject, Color.red, 2.0f);

        return angle;
    }

    // Calculate the distance from current position to finder object
    public float CalculateDistance(GameObject agentPos, Vector3 targetPos)
    {
        // Calculate the distance between the objects using pythagoras

        float distance = Mathf.Sqrt(((agentPos.transform.position.x - targetPos.x) * (agentPos.transform.position.x - targetPos.x)) + ((agentPos.transform.position.z - targetPos.z) * (agentPos.transform.position.z - targetPos.z)));

        return distance;
    }
}
