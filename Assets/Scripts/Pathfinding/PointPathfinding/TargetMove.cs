using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetMove : MonoBehaviour
{

    private PointPathfinder pointPathfind;
    private SensorScript sensorScript;

    public NodeIndexGenerator nodeIndexGen;
    private int indexOfIndexs;

    private int[] arrayOfTargetIndexs;

    public Text scoreText;

    private int targetsAcquired = 0;

    private bool firstTime = true;

    // Start is called before the first frame update
    void Start()
    {
        pointPathfind = GetComponent<PointPathfinder>();
        sensorScript = GetComponent<SensorScript>();
        pointPathfind.InitaliseNodes();

        indexOfIndexs = 0;
    }

    void ChangeTarget()
    {
        this.transform.position = pointPathfind.nodes[arrayOfTargetIndexs[indexOfIndexs]].worldPosition;
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 0.11f, this.transform.position.z);
    }
    // Update is called once per frame
    void Update()
    {
        if (firstTime == true)
        {
            arrayOfTargetIndexs = new int[nodeIndexGen.sizeOfNodeLists];
            for (int i = 0; i < nodeIndexGen.sizeOfNodeLists - 1; i++)
            {
                arrayOfTargetIndexs[i] = nodeIndexGen.arrayOfIndexs[i];
            }

            ChangeTarget();

            firstTime = false;
        }

        if (sensorScript.Hit == true)
        {
            targetsAcquired += 1;
            scoreText.text = "" + targetsAcquired;

            if (indexOfIndexs < nodeIndexGen.sizeOfNodeLists - 1)
            {
                indexOfIndexs += 1;
                ChangeTarget();
            }
            else
            {
                indexOfIndexs = 0;
                ChangeTarget();
            }
        }
    }
}
