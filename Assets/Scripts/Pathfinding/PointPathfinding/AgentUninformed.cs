using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentUninformed : MonoBehaviour
{
    // Creates instances of sensor and FSM 
    public SensorScript sensorScript;

    public PointPathfinder pointPathfinder;
    public MovementScript move;
    public GameObject target;

    public float rotationSpeed = 10.0f;
    public float speed = 1.0f;
    public float distanceAwayFromNode = 0.3f;

    private int currentIndex;

    // Start is called before the first frame update
    void Start()
    {
        pointPathfinder.InitaliseNodes();
        CalculatePath();
    }

    // Update is called once per frame
    void Update()
    {
        if ((bool)IsTargetNotAtCachedPosition() == true)
        {
            CalculatePath();
            currentIndex = 0;
        }
        else
        {
            Move();
        }
    }

    public void Move()
    {
        if (move.CalculateDistance(this.gameObject, pointPathfinder.finalPointGraph[currentIndex].worldPosition) > distanceAwayFromNode)
        {
            // Get angle
            float angle_to_turn = move.CalculateAngle(this.gameObject, pointPathfinder.finalPointGraph[currentIndex].worldPosition);

            this.transform.Rotate(0, angle_to_turn * Time.deltaTime * rotationSpeed, 0);

            // Translate locally forward in z
            this.transform.Translate(new Vector3(0, 0, speed * Time.deltaTime), Space.Self);
        }
        else
        {
            if (currentIndex < pointPathfinder.finalPointGraph.Count - 1)
            {
                currentIndex += 1;
            }
        }
    }

    public void CalculatePath()
    {
        pointPathfinder.BreadthFirstSearch(this.transform.position, target.transform.position);
    }

    public bool IsTargetNotAtCachedPosition()
    {
        Point targetClosestNode = pointPathfinder.GetClosestNode(target.transform.position);

        if (pointPathfinder.cachedTargetPoint.id != targetClosestNode.id)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    private void OnDrawGizmos()
    {
        if (pointPathfinder.finalPointGraph != null)
        {
            foreach (Point node in pointPathfinder.finalPointGraph)
            {
                Gizmos.DrawWireSphere(node.worldPosition, 0.5f);
            }
        }
    }
}
