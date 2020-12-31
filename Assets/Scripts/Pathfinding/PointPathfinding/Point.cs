using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point
{
    public Vector3 worldPosition;

    // Cost to reach this point so far
    public float gCost;
    // Estimated cost to reach target from this point
    public float hCost;
    // Parent node to this one. (Tree structure)
    public Point parent;
    // An unique identifier to the point
    public int id;

    public Point(Vector3 _worldPos, int _id)
    {
        worldPosition = _worldPos;
        id = _id;
    }

    // Total estimated cost of path through to this point
    public float fCost
    {
        get
        {
            return gCost + hCost;
        }
    }
}
