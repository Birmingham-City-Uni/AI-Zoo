using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PandaSemiIntelligent : Panda
{
    public PandaSemiIntelligent(string _id, GameObject _panda, float _food, float _water, float _awakeness, Animator _anim, NavMeshAgent _agent)
    : base(_id, _panda, _food, _water, _awakeness, _anim, _agent)
    {

    }

    // Overidden function that generates weights in a non deterministic approach still favouring more necessairy tasks
    public override void GenerateWeights()
    {
        // Array for each need
        // Food index 0,  Water index 1, sleep index 2

        if (food >= water && food >= awakeness)
        {
            if (awakeness >= water)
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
        else if (water >= food && water >= awakeness)
        {
            if (awakeness >= food)
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
        SelectTask(16);
    }

    public override bool IsNotBusy()
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
