using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunnySteering : MonoBehaviour
{
    Vector3 targetPosition;

    // Enclosure sizes
    public float xHigherBound, xLowerBound;
    public float zHigherBound, zLowerBound;

    // Timer when the bunny is in idle
    private float idleTimer;
    private float currentIdleTime;

    public float speed;

    public Transform Player;
    private Animator anim;

    // The states that the bunny can be in
    private enum BunnyState
    {
        Idle = 0,
        Seek = 1,
        Flee = 2
    }

    private BunnyState currentState;

    // Start is called before the first frame update
    void Start()
    {
        targetPosition.y = this.transform.position.y;
        currentState = BunnyState.Idle;
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // Checks if bunny is near the player
        if (Vector3.Distance(new Vector3(this.transform.position.x, 0.0f, this.transform.position.z),
                             new Vector3(Player.transform.position.x, 0.0f, Player.transform.position.z)) < 5.0f)
        {
            // If near the player the bunny flees
            currentState = BunnyState.Flee;
        }
        // If no longer close to the player and the bunny is in flee state. The state is set to seek
        else if (currentState == BunnyState.Flee)
        {
            currentState = BunnyState.Seek;
        }

        // Chooses the current task based on the state
        switch (currentState)
        {
            // If idle the timer is incremented until it has exceeded the time. Then a new target is chosen
            case BunnyState.Idle:
                if (currentIdleTime < idleTimer)
                {
                    Idle();
                    currentIdleTime += Time.deltaTime;
                }
                else
                {
                    currentState = BunnyState.Seek;
                    ChooseTarget();
                }
                break;
            case BunnyState.Seek:
                Seek();
                break;
            case BunnyState.Flee:
                Flee();
                break;
        }
    }

    void Idle()
    {
        // Play animation
        anim.Play("Idle");
    }

    void Flee()
    {
        anim.Play("Jump");
        // Gets direction away from the player
        Vector3 direction = transform.position - Player.transform.position;

        if (direction.magnitude < 5.0f)
        {
            // Rotates bunny towards the direction away from the player
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 2.0f * Time.deltaTime);
            // creates a new vector at a quicker speed than normal
            Vector3 moveVector = direction.normalized * (speed*4)  * Time.deltaTime;
            // Sets new position
            transform.position += new Vector3(moveVector.x, 0, moveVector.z);
        }
    }

    // Creates a new random target position within the enclosure
    void ChooseTarget()
    {
        targetPosition.x = Random.Range(xLowerBound, xHigherBound);
        targetPosition.z = Random.Range(zLowerBound, zHigherBound);
    }

    void Seek()
    {
        // Plays animations
        anim.Play("Walk");

        // Gets direction towards the target
        Vector3 direction = targetPosition - this.transform.position;

        // Rotates the bunny towards the target
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime);

        // Checks if bunnt is at the target
        if (direction.magnitude > 0.5f)
        {
            // Creates new vector
            Vector3 moveVector = direction.normalized * speed * Time.deltaTime;

            transform.position += moveVector;

        }
        else
        {
            // Sets state to idle creates new idle time and resets the current timer
            currentState = BunnyState.Idle;
            idleTimer = Random.Range(5.0f, 10.0f);
            currentIdleTime = 0.0f;
        }
    }

}
