using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeIndexGenerator : MonoBehaviour
{
    public GameObject nodeListParent1;
    public GameObject nodeListParent2;

    bool identicalNodeList = false;

    public int[] arrayOfIndexs;
    public int sizeOfNodeLists;

    // Start is called before the first frame update
    void Start()
    {
        sizeOfNodeLists = nodeListParent1.transform.childCount;

        if (sizeOfNodeLists == (nodeListParent2.transform.childCount))
        {
            identicalNodeList = true;
        }
        else
        {
            Debug.Log("Node lists not identical");
        }

        if (identicalNodeList == true)
        {
            arrayOfIndexs = new int[sizeOfNodeLists];
            for (int i = 0; i< sizeOfNodeLists - 1; i++)
            {
                arrayOfIndexs[i] = i;
            }

            Shuffle();
        }
    }

    void Shuffle()
    {
        for (int i = 0; i < 100; i++)
        {
            int indexTo = Random.Range(0, sizeOfNodeLists - 1);
            int indexFrom = Random.Range(0, sizeOfNodeLists - 1);

            int Placeholder = arrayOfIndexs[indexFrom];
            arrayOfIndexs[indexFrom] = arrayOfIndexs[indexTo];
            arrayOfIndexs[indexTo] = Placeholder;
        }
    }

}
