using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Panda
{

    // Internal parameters, Overwritten in class constructor
    public float food = 0;
    public float water = 0;
    public float awakeness = 0;
    private float speed = 0;
    private string id = "";
    private Animator anim;
    GameObject panda;

    private GameObject target;

    // Creates a movemoment script object
    MovementScript moveScript;

    // Target contains values for all carried out tasks
    public enum Target
    {
        noTask,
        food,
        water,
        shelter
    }

    // task variable stores the current selected task
    public Target task = Target.noTask;

    // Class constuctor
    public Panda(string _id, GameObject _panda, float _food, float _water, float _awakeness, float _speed, Animator _anim, MovementScript _moveScript)
    {
        id = _id;
        panda = _panda;
        food = _food;
        water = _water;
        awakeness = _awakeness;
        speed = _speed;
        anim = _anim;
        moveScript = _moveScript;
    }

    // Sets task with given target position and task type
    public void SetTask(GameObject _target, Target _task)
    {
        target = _target;
        task = _task;
    }

    // Moves the panda
    private void Move()
    {
        // Plays walk animation
        anim.Play("Walk");

        // Gets angle that the panda needs to turn in order to face the target
        float angle_to_turn = moveScript.CalculateAngle(panda.gameObject, target.transform.position);

        // Rotates the panda
        panda.transform.Rotate(0, angle_to_turn * Time.deltaTime * 10.0f, 0);

        // Translate locally forward in z
        panda.transform.Translate(new Vector3(0, 0, speed * Time.deltaTime), Space.Self);
    }

    // Decay of pandas 3 parameters. Called within update
    private void Decay()
    {
        food -= 1.0f * Time.deltaTime;
        water -= 1.0f * Time.deltaTime;
        awakeness -= 1.5f * Time.deltaTime;
    }

    // Called every frame to update pandas parameters and move panda
    public void Update()
    {
        Decay();

        // Checks if the panda currently has a task
        if (task != Target.noTask)
        {
            // Calculates the distance between the target and panda
            if (moveScript.CalculateDistance(panda.gameObject, target.transform.position) > 0.5f)
            {
                // If panda is not at the target the panda moves towards it
                Move();
            }
            // Panda at target
            else
            {
                // Task is food
                if (task == Target.food)
                {
                    // Eats food until above 99
                    if (food < 99.0f)
                    {
                        // Plays animation
                        anim.Play("Hit");
                        // Eats food
                        food += 0.24f;
                    }
                    else
                    {
                        // Resets task to noTask
                        task = Target.noTask;
                    }
                }
                else if (task == Target.water)
                {
                    if (water < 99.0f)
                    {
                        anim.Play("Attack");
                        water += 0.24f;
                    }
                    else
                    {
                        task = Target.noTask;
                    }
                }
                else
                {
                    if (awakeness < 99.0f)
                    {
                        anim.Play("Idle2");
                        awakeness += 0.12f;
                    }
                    else
                    {
                        task = Target.noTask;
                    }
                }
            }
        }
    }

    // Checks current need of the panda
    // Sets weights for each to prioritise more important tasks
    public virtual int[] CheckNeed()
    {
        // Array for each need
        // Food index 0,  Water index 1, heat index 3
        int[] needs = new int[3];

        for (int i = 0; i < needs.Length; i++)
        {
            needs[i] = Random.Range(0, 5);
        }

        return needs;
    }

    // Called to check if panda has a task or not
    public bool IsNotBusy()
    {
        if (task == Target.noTask)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
