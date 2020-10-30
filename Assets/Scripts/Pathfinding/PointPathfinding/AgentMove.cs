using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentMove : MonoBehaviour
{
    public PointPathfinder PointPathfinder;
    public MovementScript move;
    public GameObject target;
    private Rigidbody rb;

    public float autoRotationSpeed = 10.0f;
    public float speed = 1.0f;
    public float distanceAway = 0.2f;

    int currentIndex;

    private void Start()
    {
        PointPathfinder.InitaliseNodes();
        PointPathfinder.FindPath(this.transform.position, target.transform.position);
        currentIndex = 0;
        rb.GetComponent<Rigidbody>();
    }

    void Move(Vector3 targetPos)
    {
        // Get distance and angle
        float distance = move.CalculateDistance(this.gameObject, targetPos);
        float angle_to_turn = move.CalculateAngle(this.gameObject, targetPos);

        this.transform.Rotate(0, angle_to_turn * Time.deltaTime * autoRotationSpeed, 0);

        // Translate locally forward in z
        this.transform.Translate(new Vector3(0, 0, speed * Time.deltaTime), Space.Self);

    }

    private void Update()
    {
        if (Vector3.Distance(this.transform.position, target.transform.position) > distanceAway)
        {
            if (Vector3.Distance(this.transform.position, PointPathfinder.finalPointGraph[currentIndex].worldPosition) > distanceAway)
            {

                Move(PointPathfinder.finalPointGraph[currentIndex].worldPosition);
            }
            else
            {
                if (currentIndex < PointPathfinder.finalPointGraph.Count)
                {
                    currentIndex += 1;
                }
            }
        }
    }
}
