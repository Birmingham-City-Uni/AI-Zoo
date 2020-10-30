using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphViz : MonoBehaviour
{

    GameObject[] nodes;
    public float distanceThreshold = 1.0f;
    public float stepDivider = 0.1f;
    public float surfaceBelowDistance = 0.1f;

    List<Vector3> links;
    List<Vector3> from_pos;

    // Start is called before the first frame update
    void Start()
    {
        // Initalise array with size of elements in given nodes
        int numElements = this.transform.childCount;
        nodes = new GameObject[numElements];

        // Create dynamic storage for edges and position from which edge starts
        links = new List<Vector3>();
        from_pos = new List<Vector3>();

        // Initalise array values to child objects
        for (int i = 0; i < numElements; i++)
        {
            nodes[i] = this.transform.GetChild(i).gameObject;
        }

        // Check each node again each other node
        foreach (GameObject node1 in nodes)
        {
            foreach (GameObject node2 in nodes)
            {
                // Check if nodes are different
                if (node1 != node2)
                {
                    // Calc distance between the 2 nodes
                    float distance = Vector3.Distance(node1.transform.position, node2.transform.position);

                    // Check if distance is below pre determined threshold
                    if (distance < distanceThreshold)
                    {
                        // Get vector between nodes
                        Vector3 edge = node2.transform.position - node1.transform.position;

                        // Checks if path is walkable. i.e if there is an object beneath to walk on
                        bool walkable = true;

                        // Step between the path in stages of stepDivider
                        int steps = Convert.ToInt32(edge.magnitude / stepDivider);

                        // Iterate though steps
                        for (int i = 0; i < steps; i++)
                        {
                            // Step through the distance between nodes
                            Vector3 pos = node1.transform.position + edge.normalized * (i / steps);
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
                        if (Physics.Raycast(node1.transform.position, edge, distanceThreshold) == false && walkable == true)
                        {
                            // Add edge to list as well as the position of the edge start
                            links.Add(edge);
                            from_pos.Add(node1.transform.position);
                        }
                    }

                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        int i = 0;
        foreach (Vector3 link in links)
        {
            Debug.DrawLine(from_pos[i], from_pos[i] + link, Color.red);
            i++;
        }
    }
}
