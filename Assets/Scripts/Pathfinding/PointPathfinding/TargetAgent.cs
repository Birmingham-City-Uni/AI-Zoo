using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Target agent can be given to an agent to make it pathfind randomly around a point graph

public class TargetAgent : MonoBehaviour
{

    public PointPathfinder pointPathfinder;
    public MovementScript move;
    private Point targetPoint;

    public float rotationSpeed = 10.0f;
    public float speed = 1.0f;
    public float distanceAwayFromNode = 0.3f;
    private int index;

    // Start is called before the first frame update
    void Start()
    {
        pointPathfinder.InitaliseNodes();
        targetPoint = pointPathfinder.GetRandomPoint();
        pointPathfinder.FindPath(this.transform.position, targetPoint.worldPosition);
        index = 0;
    }

    private void Move()
    {
        // Get angle
        float angle_to_turn = move.CalculateAngle(this.gameObject, pointPathfinder.finalPointGraph[index].worldPosition);

        this.transform.Rotate(0, angle_to_turn * Time.deltaTime * rotationSpeed, 0);

        // Translate locally forward in z
        this.transform.Translate(new Vector3(0, 0, speed * Time.deltaTime), Space.Self);
    }

    // Update is called once per frame
    void Update()
    {
        // If not at target
        if (move.CalculateDistance(this.gameObject, targetPoint.worldPosition) > distanceAwayFromNode)
        {
            // If not at next node
            if (move.CalculateDistance(this.gameObject, pointPathfinder.finalPointGraph[index].worldPosition) > distanceAwayFromNode)
            {
                Move();
            }
            else
            {
                index += 1;
            }
        }
        else
        {
            targetPoint = pointPathfinder.GetRandomPoint();
            pointPathfinder.FindPath(this.transform.position, targetPoint.worldPosition);
            index = 0;
        }
    }
}
