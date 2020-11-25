using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class PointPathfinder : MonoBehaviour
{
    // Legal distance between each point
    public float distanceThreshold = 1.9f;
    // Step when checking for object beneath
    public float stepDivider = 0.01f;
    // Distance below a path that allows it to be a legal movement
    public float surfaceBelowDistance = 0.1f;

    // Parent object to scene nodes and array to store all nodes
    public GameObject nodeList;
    public Point[] nodes;

    // List of points the agent needs to follow to get to its goal
    public List<Point> finalPointGraph;

    // Cached point that was closest to the target on the last path calculation
    public Point cachedTargetPoint;

    // Create nodes from given parent node and store them in the nodes array
    public void InitaliseNodes()
    {
        int numElements = nodeList.transform.childCount;
        nodes = new Point[numElements];

        for (int i = 0; i < numElements; i++)
        {
            nodes[i] = new Point(nodeList.transform.GetChild(i).gameObject.transform.position, i);
        }
    }

    // Get connecting points which are legal to move to
    List<Point> GetNeighbourNodes(Point currentNode)
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

    // Get a random point for a target to traverse to
    public Point GetRandomPoint()
    {
        if (nodes.Length > 0)
        {
            int randomInt = UnityEngine.Random.Range(0, nodes.Length - 1);
            return nodes[randomInt];
        }
        else
        {
            return null;
        }
    }

    // Get closest node to a given vector
    public Point GetClosestNode(Vector3 startingPosition)
    {
        Point closestNode = new Point(Vector3.zero, 0);
        float closestDistance = 100.0f;

        // Iterate through all nodes
        foreach (Point node in nodes)
        {
            // Check if distance is less that previous shortest distance
            if (closestDistance > Vector3.Distance(startingPosition, node.worldPosition))
            {
                closestNode = node;
                closestDistance = Vector3.Distance(startingPosition, node.worldPosition);
            }
        }
        return closestNode;
    }

    // Retrace through parent nodes, forming an ordered list of nodes for the agent to traverse through to get to their target
    void RetracePath(Point startNode, Point endNode)
    {
        // List that will store the desired path
        List<Point> path = new List<Point>();
        // Start at the end node (target)
        Point currentNode = endNode;

        // Check were not at the start node
        while (currentNode != startNode)
        {
            // Add node to the list
            path.Add(currentNode);
            // Get the parent node
            currentNode = currentNode.parent;
        }
        // Flip the list to give us an ordered list from start to finish
        path.Reverse();

        finalPointGraph = path;
    }

    // A* pathfind. Takes start and end position and returns the optimal path to the target
    public void FindPath(Vector3 startingPos, Vector3 finishPos)
    {
        Debug.Log("Finding Path");
        // openSet is a list of points to currently explore
        List<Point> openSet = new List<Point>();
        // Closed set is a set of points that have been explored
        HashSet<Point> closedSet = new HashSet<Point>();

        // Get closest nodes to the starting and finishing position
        Point startingPoint = GetClosestNode(startingPos);
        Point targetPoint = GetClosestNode(finishPos);
        Debug.Log("Target point location: " + targetPoint.worldPosition);
        // Cache the target position which will be used externally to test for a moving target
        cachedTargetPoint = targetPoint;

        // Add starting point to open set
        openSet.Add(startingPoint);

        // Keeps calculating path unless no path can be formed
        while (openSet.Count > 0)
        {
            // Sets node to first in open set
            Point node = openSet[0];

            // Iterates through all nodes in current open set, setting node equal to any point that has a lower estimated cost and lower total cost
            for (int i = 1; i < openSet.Count; i++)
            {
                // Checks if this point has a lower total estimated cost than the current node
                if (openSet[i].fCost <= node.fCost)
                {
                    // Checjs if this point has a lower estimated cost to the goal
                    if (openSet[i].hCost < node.hCost)
                        // Sets the node equal to the lower costing node in both estimated and total cost
                        node = openSet[i];
                }
            }
            // Removes the node from the set
            openSet.Remove(node);
            // Add that node to the closed set
            closedSet.Add(node);

            // Evaluates true when the node is the target point
            if (node.id == targetPoint.id)
            {
                Debug.Log("Retrace Paths");
                // Retrace through the parents of each node from target to start giving a final path to follow
                RetracePath(startingPoint, targetPoint);
                return;
            }

            // Looks through each point that is a neightbour to the node
            foreach (Point neighbour in GetNeighbourNodes(node))
            {
                // Checks if closed set already has the neighbour point
                if (closedSet.Contains(neighbour))
                {
                    // If so goes to next neighbour in for each
                    continue;
                }
                // Works out the cost to get to this neighbour node, using the gCost (cost of getting to the node prior to this) + the distance between the current node and the neighbour node
                float newCostToNeighbour = node.gCost + Vector3.Distance(node.worldPosition, neighbour.worldPosition);
                // Evaluates true when the cost to get to the neighbour from that node is lower than the cost of getting to neighbour so far
                // OR when the openset doesnt contain this neighbour
                if (newCostToNeighbour < neighbour.gCost || openSet.Contains(neighbour) == false)
                {
                    // Sets the neighbours cost to get to this node to the above calculated cost
                    neighbour.gCost = newCostToNeighbour;
                    // The esitmated cost is calculated by giving the function the neighbours position and the targets position
                    neighbour.hCost = Vector3.Distance(neighbour.worldPosition, targetPoint.worldPosition);
                    // The neighbour is given the parent which it is following on from
                    neighbour.parent = node;
                    // If the open set doesnt already contain the neighbour
                    if (openSet.Contains(neighbour) == false)
                        // The neighbour is added to the open set
                        openSet.Add(neighbour);
                }
            }
        }
    }

    // Breadth first search, from a starting position to a finish position
    public void BreadthFirstSearch(Vector3 startingPos, Vector3 finishPos)
    {
        // What is currently being explored
        Queue<Point> openSet = new Queue<Point>();
        // Already explored point list
        List<Point> closedSet = new List<Point>();

        // Get the closest starting and finishing points
        Point startingPoint = GetClosestNode(startingPos);
        Point targetPoint = GetClosestNode(finishPos);

        // Cache a point at which the target started at
        cachedTargetPoint = targetPoint;

        // Add the starting point to the front of the queue
        openSet.Enqueue(startingPoint);

        // Continue until a path is found
        while (openSet.Count != 0)
        {
            // Get the first point in thr queue
            Point point = openSet.Dequeue();
            // Add the point to the explored list
            closedSet.Add(point);

            // Check if this point is the target point
            if (point.id == targetPoint.id)
            {
                // If it is then create the path to the target from the start
                RetracePath(startingPoint, targetPoint);
                return;
            }

            // Iterate through a points neighbours
            foreach (Point neighbour in GetNeighbourNodes(point))
            {
                // Check if the explored point list already contains this point
                if (closedSet.Contains(neighbour))
                {
                    // If so, go to the next element in the foreach
                    continue;
                }
                // If the currently explored set does not contain this neighbour
                if (openSet.Contains(neighbour) == false)
                {
                    // Add the neighbout to the queue
                    openSet.Enqueue(neighbour);
                    // Set the neighbours parent to be the point
                    neighbour.parent = point;
                }
            }
        }
    }

    //float GetDistance(Vector3 pointA, Vector3 pointB)
    //{
    //    // Get distance between points (Not taking into account the y, because height does not impact the time to get to the next node
    //    float distanceX = Mathf.Abs(pointA.x - pointB.x);
    //    float distanceZ = Mathf.Abs(pointA.z - pointB.z);

    //    // Check which distance is greater
    //    if (distanceX > distanceZ)
    //    {
    //        return (14 * distanceZ) + 10 * (distanceX - distanceZ);
    //    }
    //    else
    //    {
    //        return (14 * distanceX) + 10 * (distanceZ - distanceX);
    //    }
    //}
}
