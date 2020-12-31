using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetMove : MonoBehaviour
{

    // Pathfind, sensors and node index generator variables
    private PointPathfinder pointPathfind;
    private SensorScript sensorScript;
    public NodeIndexGenerator nodeIndexGen;
    private int indexOfIndexs;

    private int[] arrayOfTargetIndexs;

    // UI elements
    public Text scoreText;
    private int targetsAcquired = 0;

    private bool firstTime = true;

    // Start is called before the first frame update
    void Start()
    {
        // Gets and initialises node array
        pointPathfind = GetComponent<PointPathfinder>();
        sensorScript = GetComponent<SensorScript>();
        pointPathfind.InitaliseNodes();

        indexOfIndexs = 0;
    }

    // Called to change the target to the next index on from it
    void ChangeTarget()
    {
        // Moves target agent to the given position in the array of target indexs
        this.transform.position = pointPathfind.nodes[arrayOfTargetIndexs[indexOfIndexs]].worldPosition;
        // Accounts for node height
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 0.11f, this.transform.position.z);
    }
    // Update is called once per frame
    void Update()
    {
        if (firstTime == true)
        {
            // Initialises the index and chooses the first target
            arrayOfTargetIndexs = new int[nodeIndexGen.sizeOfNodeLists];
            for (int i = 0; i < nodeIndexGen.sizeOfNodeLists - 1; i++)
            {
                arrayOfTargetIndexs[i] = nodeIndexGen.arrayOfIndexs[i];
            }

            ChangeTarget();

            firstTime = false;
        }

        // When the agent gets to this target it goes to the next position
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
