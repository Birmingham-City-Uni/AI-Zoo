using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentUninformed : MonoBehaviour
{

    private PointPathfinder pointPathfinder;
    private MovementScript move;
    private Animator anim;
    public GameObject target;

    public float rotationSpeed = 10.0f;
    public float speed = 1.0f;
    public float distanceAwayFromNode = 0.3f;

    private int currentIndex;

    // Start is called before the first frame update
    void Start()
    {
        pointPathfinder = GetComponent<PointPathfinder>();
        move = GetComponent<MovementScript>();
        anim = GetComponent<Animator>();
        anim.speed = 0.5f;
        // Initialise node graph
        pointPathfinder.InitaliseNodes();
        // Calculate the initial path
        CalculatePath();
    }

    // Update is called once per frame
    void Update()
    {
        // Checks if the target is different
        if ((bool)IsTargetNotAtCachedPosition() == true)
        {
            CalculatePath();
            currentIndex = 0;
        }
        else
        {
            // Plays animation and moves toward target
            anim.Play("Fly Inplace");
            Move();
        }
    }

    // Called to move the agent towards its target
    public void Move()
    {
        // Checks if agent is at the target location
        if (move.CalculateDistance(this.gameObject, pointPathfinder.finalPointGraph[currentIndex].worldPosition) > distanceAwayFromNode)
        {
            // Gets turn angle
            float angle_to_turn = move.CalculateAngle(this.gameObject, pointPathfinder.finalPointGraph[currentIndex].worldPosition);

            // Rotates the agent towards its target
            this.transform.Rotate(0, angle_to_turn * Time.deltaTime * rotationSpeed, 0);

            // Translate locally forward in z
            this.transform.Translate(new Vector3(0, 0, speed * Time.deltaTime), Space.Self);
        }
        else
        {
            // Increments index to next point in the graph
            if (currentIndex < pointPathfinder.finalPointGraph.Count - 1)
            {
                currentIndex += 1;
            }
        }
    }

    public void CalculatePath()
    {
        // Calls the breadth first search algorithm with agents position and targets position
        pointPathfinder.BreadthFirstSearch(this.transform.position, target.transform.position);
    }

    // Returns true when target has moved
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

    // Debug Gizmos that shows the current path to the target
    private void OnDrawGizmos()
    {
        if (Application.isPlaying == true)
        {
            if (pointPathfinder.finalPointGraph != null)
            {
                foreach (Point node in pointPathfinder.finalPointGraph)
                {
                    Gizmos.color = Color.cyan;
                    Gizmos.DrawWireSphere(node.worldPosition, 0.5f);
                }
            }
        }
    }
}
