using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Panda
{

    // Internal parameters, Overwritten in class constructor
    public float food = 0;
    public float water = 0;
    public float awakeness = 0;
    private string id = "";
    private Animator anim;
    GameObject panda;
    protected NavMeshAgent agent;
    protected int[] needs;
    protected Target selectedTask;
    private GameObject target;


    // Target contains values for all tasks
    public enum Target
    {
        noTask,
        food,
        water,
        shelter
    }

    // Task variable stores the current selected task
    public Target task = Target.noTask;

    // Class constuctor
    public Panda(string _id, GameObject _panda, float _food, float _water, float _awakeness, Animator _anim, NavMeshAgent _agent)
    {
        id = _id;
        panda = _panda;
        food = _food;
        water = _water;
        awakeness = _awakeness;
        anim = _anim;
        agent = _agent;
        // Initialises needs array
        needs = new int[3];

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

        // Sets nav mesh destination
        agent.destination = target.transform.position;
    }

    // Decay of pandas 3 parameters. Called within update
    private void Decay()
    {
        food -= 0.75f * Time.deltaTime;
        water -= 0.75f * Time.deltaTime;
        awakeness -= 1.0f * Time.deltaTime;
    }

    // Called every frame to update pandas parameters and move panda
    public void Update()
    {
        Decay();

        // Checks if the panda currently has a task
        if (task != Target.noTask)
        {
            // Calculates the distance between the target and panda
            if (Vector3.Distance(panda.gameObject.transform.position, target.transform.position) > 1.5f)
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
                    // Eats food until full
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

    // Checks current needs of the panda
    // Sets weights for each need to prioritise more important tasks
    // Initial implementation returns completely random weights
    public virtual void GenerateWeights()
    {
        // Array for each need
        // Food index 0,  Water index 1, heat index 3

        int totalRange = 0;

        for (int i = 0; i < needs.Length; i++)
        {
            needs[i] = Random.Range(0, 5);
            totalRange += needs[i];
        }
        SelectTask(totalRange);
    }

    // Selects tasks with the input parameter of weights array n1 + n2 + n3
    protected void SelectTask(int totalRange)
    {
        int rand = Random.Range(0, totalRange);

        // Selects tasks from needs array 
        if (rand < needs[0])
        {
            selectedTask = Target.food;
        }
        else if (rand < needs[1])
        {
            selectedTask = Target.water;
        }
        else
        {
            selectedTask = Target.shelter;
        }
    }

    // Called by behaviour tree to check which task should be carried out next
    public bool PandaShouldEat()
    {
        if (selectedTask == Target.food)
        {
            return true;
        }
        return false;
    }
    public bool PandaShouldDrink()
    {
        if (selectedTask == Target.water)
        {
            return true;
        }
        return false;
    }
    public bool PandaShouldSleep()
    {
        if (selectedTask == Target.shelter)
        {
            return true;
        }
        return false;
    }

    // Called to check if panda has a task or not
    public virtual bool IsNotBusy()
    {
        if (task == Target.noTask)
        {
            GenerateWeights();
            return true;
        }
        else
        {
            return false;
        }
    }
}
