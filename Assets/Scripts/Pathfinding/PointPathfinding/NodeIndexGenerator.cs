using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script generates an array of random indexs in which both pathfinding enclosures will traverse
public class NodeIndexGenerator : MonoBehaviour
{
    // The parent object of all nodes in each enclosure
    public GameObject nodeListParent1;
    public GameObject nodeListParent2;

    bool identicalNodeList = false;

    public int[] arrayOfIndexs;
    public int sizeOfNodeLists;

    // Start is called before the first frame update
    void Start()
    {
        // Gets the size of each node list and checks if they are equal
        sizeOfNodeLists = nodeListParent1.transform.childCount;
        if (sizeOfNodeLists == (nodeListParent2.transform.childCount))
        {
            identicalNodeList = true;
        }
        else
        {
            // Debug an error message
            Debug.Log("Node lists not identical");
        }

        if (identicalNodeList == true)
        {
            // Initialises the array of indexs with the amount in the lists
            arrayOfIndexs = new int[sizeOfNodeLists];
            for (int i = 0; i < sizeOfNodeLists - 1; i++)
            {
                // Sets values so that n[0] = 0, n[1] = 1 ... n[x] = x 
                arrayOfIndexs[i] = i;
            }

            // Calls a shuffle algorithm
            Shuffle();
        }
    }

    // Shuffles the array 
    void Shuffle()
    {
        // Shuffles 200 times
        for (int i = 0; i < 200; i++)
        {
            // Gets 2 random indexs
            int indexTo = Random.Range(0, sizeOfNodeLists - 1);
            int indexFrom = Random.Range(0, sizeOfNodeLists - 1);

            if (indexTo != indexFrom)
            {
                // Swaps the indexs
                int Placeholder = arrayOfIndexs[indexFrom];
                arrayOfIndexs[indexFrom] = arrayOfIndexs[indexTo];
                arrayOfIndexs[indexTo] = Placeholder;
            }
        }
    }

}
