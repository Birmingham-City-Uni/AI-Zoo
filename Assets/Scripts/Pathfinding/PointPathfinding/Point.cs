using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point
{
    public Vector3 worldPosition;

    public float gCost;
    public float hCost;
    public Point parent;

    public Point(Vector3 _worldPos)
    {
        worldPosition = _worldPos;
    }

    public float fCost
    {
        get
        {
            return gCost + hCost;
        }
    }
}
