using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class PointPathfinder : MonoBehaviour
{

    public float distanceThreshold = 1.9f;
    public float stepDivider = 0.01f;
    public float surfaceBelowDistance = 0.2f;

    public GameObject nodeList;
    private Point [] nodes;

    public List<Point> finalPointGraph;

    public void InitaliseNodes()
    {
        int numElements = nodeList.transform.childCount;
        nodes = new Point[numElements];

        for (int i = 0; i < numElements; i++)
        {
            nodes[i] = new Point(nodeList.transform.GetChild(i).gameObject.transform.position, i);
        }
    }

    List<Point> GetNeighboutNodes(Point currentNode)
    {
        List<Point> neighbours = new List<Point>();

        // Check each node again each other node
        foreach (Point node in nodes)
        {
            // Check if nodes are different
            if (currentNode != node)
            {
                // Calc distance between the 2 nodes
                float distance = Vector3.Distance(currentNode.worldPosition, node.worldPosition);

                // Check if distance is below pre determined threshold
                if (distance < distanceThreshold)
                {
                    // Get vector between nodes
                    Vector3 edge = node.worldPosition - currentNode.worldPosition;

                    // Checks if path is walkable. i.e if there is an object beneath to walk on
                    bool walkable = true;

                    // Step between the path in stages of stepDivider
                    int steps = Convert.ToInt32(edge.magnitude / stepDivider);

                    // Iterate though steps
                    for (int i = 0; i < steps; i++)
                    {
                        // Step through the distance between nodes
                        Vector3 pos = currentNode.worldPosition + edge.normalized * (i / steps);
                        RaycastHit hit;
                        // Check if down raycast hits a surface
                        if (Physics.Raycast(pos, Vector3.down, out hit, distanceThreshold) && walkable == true)
                        {
                            // If there isnt a surface within surfaceBelowDistance set walkable to false
                            if (hit.distance > surfaceBelowDistance)
                            {
                                walkable = false;
                            }
                        }
                    }

                    // Checks that edge does not go through other objects
                    if (Physics.Raycast(currentNode.worldPosition, edge, distanceThreshold) == false && walkable == true)
                    {
                        // Add edge to list as well as the position of the edge start
                        neighbours.Add(node);
                    }
                }
            }
        }
        return neighbours;
    }

    Point GetClosestNode(Vector3 startingPosition)
    {
        Point closestNode = new Point(Vector3.zero, 0);
        float closestDistance = 100.0f;

        foreach(Point node in nodes)
        {
            if (closestDistance > Vector3.Distance(startingPosition, node.worldPosition))
            {
                closestNode = node;
                closestDistance = Vector3.Distance(startingPosition, node.worldPosition);
            }
        }
        return closestNode;
    }

    void RetracePath(Point startNode, Point endNode)
    {
        Debug.Log("Retrace");
        List<Point> path = new List<Point>();
        Point currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        finalPointGraph = path;
    }

    public void FindPath(Vector3 startingPos, Vector3 finishPos)
    {
        List<Point> openSet = new List<Point>();
        HashSet<Point> closedSet = new HashSet<Point>();

        Point startingPoint = GetClosestNode(startingPos);
        Point targetPoint = GetClosestNode(finishPos);
        openSet.Add(startingPoint);

        while (openSet.Count > 0)
        {
            Point node = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost <= node.fCost)
                {
                    if (openSet[i].hCost < node.hCost)
                        node = openSet[i];
                }
            }

            openSet.Remove(node);
            closedSet.Add(node);
            if (node.id == targetPoint.id)
            {
                RetracePath(startingPoint, targetPoint);
                return;
            }
            foreach (Point neighbour in GetNeighboutNodes(node))
            {
                if (closedSet.Contains(neighbour))
                {
                    continue;
                }
                float newCostToNeighbour = node.gCost + Vector3.Distance(node.worldPosition, neighbour.worldPosition);
                if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newCostToNeighbour;
                    neighbour.hCost = Vector3.Distance(neighbour.worldPosition, targetPoint.worldPosition);
                    neighbour.parent = node;
                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }
    }

}
