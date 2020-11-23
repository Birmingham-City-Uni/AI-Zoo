using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;


public class AgentAStar : MonoBehaviour
{
    // Creates instances of sensor and FSM 
    public SensorScript sensorScript;
    private FSMStateManager stateManager;
    public SeekState seekState;
    public IdleState idleState;
    public CalculatePathState calculatePathState;

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
        // Sets state to seek
        stateManager = new FSMStateManager();
        idleState = new IdleState(this, stateManager);
        seekState = new SeekState(this, stateManager);
        calculatePathState = new CalculatePathState(this, stateManager);
        stateManager.Init(idleState);
        pointPathfinder.InitaliseNodes();
    }

    // Update is called once per frame
    void Update()
    {
        // Calls state update
        stateManager.Update();
        // Calls state selector
        StateSelector();
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
        pointPathfinder.FindPath(this.transform.position, target.transform.position);
        float distanceToSecondPoint = Vector3.Distance(this.transform.position, pointPathfinder.finalPointGraph[1].worldPosition);
        if (distanceToSecondPoint < pointPathfinder.distanceThreshold &&
            Mathf.Abs(pointPathfinder.finalPointGraph[0].worldPosition.y - pointPathfinder.finalPointGraph[1].worldPosition.y) < 0.1f)
        {
            currentIndex = 1;
        }
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

    void StateSelector()
    {
        if (stateManager.GetCurrentState().GetType() == typeof(CalculatePathState))
        {
            stateManager.PopState();
            stateManager.PushState(seekState);
        }

        if (stateManager.GetCurrentState().GetType() == typeof(IdleState) && sensorScript.Hit == false)
        {
            stateManager.PopState();
            stateManager.PushState(calculatePathState);
            currentIndex = 0;
        }

        if (sensorScript.Hit == true && stateManager.GetCurrentState().GetType() == typeof(SeekState))
        {
            stateManager.PopState();
            stateManager.PushState(idleState);
        }
    }

    private void OnDrawGizmos()
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
