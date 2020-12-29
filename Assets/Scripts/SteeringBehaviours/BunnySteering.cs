using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunnySteering : MonoBehaviour
{
    Vector3 targetPosition;

    public float xHigherBound, xLowerBound;
    public float zHigherBound, zLowerBound;

    private float idleTimer;
    private float currentIdleTime;

    public float speed;

    public Transform Player;
    private Animator anim;

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
        if (Vector3.Distance(new Vector3(this.transform.position.x, 0.0f, this.transform.position.z),
                             new Vector3(Player.transform.position.x, 0.0f, Player.transform.position.z)) < 5.0f)
        {
            currentState = BunnyState.Flee;
        }
        else if (currentState == BunnyState.Flee)
        {
            currentState = BunnyState.Seek;
        }

        switch (currentState)
        {
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
        Vector3 direction = transform.position - Player.transform.position;
        if (direction.magnitude < 8.0f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 2.0f * Time.deltaTime);
            Vector3 moveVector = direction.normalized * (speed*4)  * Time.deltaTime;
            transform.position += new Vector3(moveVector.x, 0, moveVector.z);
        }
    }

    void ChooseTarget()
    {
        targetPosition.x = Random.Range(xLowerBound, xHigherBound);
        targetPosition.z = Random.Range(zLowerBound, zHigherBound);
    }

    void Seek()
    {
        anim.Play("Walk");
        Vector3 direction = targetPosition - this.transform.position;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime);

        if (direction.magnitude > 0.5f)
        {

            Vector3 moveVector = direction.normalized * speed * Time.deltaTime;

            transform.position += moveVector;

        }
        else
        {
            currentState = BunnyState.Idle;
            idleTimer = Random.Range(5.0f, 10.0f);
            currentIdleTime = 0.0f;
        }
    }

}
