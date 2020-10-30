using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point
{
    public Vector3 worldPosition;

    public float gCost;
    public float hCost;
    public Point parent;
    public int id;

    public Point(Vector3 _worldPos, int _id)
    {
        worldPosition = _worldPos;
        id = _id;
    }

    public float fCost
    {
        get
        {
            return gCost + hCost;
        }
    }
}
