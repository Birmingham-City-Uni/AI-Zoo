using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Panda
{

    // Internal parameters, Overwritten in class constructor
    private float food = 0;
    private float water = 0;
    private float temperature = 0;
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
    private Target task = Target.noTask;

    // Class constuctor
    public Panda(string _id, GameObject _panda, float _food, float _water, float _temperature, float _speed, Animator _anim, MovementScript _moveScript)
    {
        id = _id;
        panda = _panda;
        food = _food;
        water = _water;
        temperature = _temperature;
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
        food -= 2.0f * Time.deltaTime;
        water -= 2.0f * Time.deltaTime;
        temperature -= 2.5f * Time.deltaTime;
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
                        food += 0.08f;
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
                        water += 0.08f;
                    }
                    else
                    {
                        task = Target.noTask;
                    }
                }
                else
                {
                    if (temperature < 99.0f)
                    {
                        anim.Play("Idle2");
                        temperature += 0.04f;
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
    public int[] CheckNeed()
    {
        // Array for each need
        // Food index 0,  Water index 1, heat index 3
        int[] needs = new int[3];

        // Checks what animal currently needs more
        if (food >= water && food >= temperature)
        {
            if (temperature >= water)
            {
                // food > heat > water
                needs[0] = 1;
                needs[1] = 10;
                needs[2] = 5;
            }
            else
            {
                // food > water > heat
                needs[0] = 1;
                needs[1] = 5;
                needs[2] = 10;
            }
        }
        else if (water >= food && water >= temperature)
        {
            if (temperature >= food)
            {
                // water > heat > food
                needs[0] = 10;
                needs[1] = 1;
                needs[2] = 5;
            }
            else
            {
                // water > food > heat
                needs[0] = 5;
                needs[1] = 1;
                needs[2] = 10;
            }
        }
        else
        {
            if (water >= food)
            {
                // temperature > water > food
                needs[0] = 10;
                needs[1] = 5;
                needs[2] = 1;
            }
            else
            {
                // temperature > food > water
                needs[0] = 5;
                needs[1] = 10;
                needs[2] = 1;
            }
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
