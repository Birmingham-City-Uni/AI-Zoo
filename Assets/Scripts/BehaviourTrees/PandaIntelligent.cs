using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PandaIntelligent : Panda
{

    public PandaIntelligent(string _id, GameObject _panda, float _food, float _water, float _awakeness, Animator _anim, NavMeshAgent _agent)
        : base(_id, _panda, _food, _water, _awakeness, _anim, _agent)
    {

    }

    public override void GenerateWeights()
    {
        // Array for each need
        // Food index 0,  Water index 1, sleep index 2

        // Gives weighting to most important task
        if (food < water && food < awakeness)
        {
            needs[0] = 100;
            needs[1] = 1;
            needs[2] = 1;
        }
        else if (water < food && water < awakeness)
        {
            needs[0] = 1;
            needs[1] = 100;
            needs[2] = 1;
        }
        else
        {
            needs[0] = 1;
            needs[1] = 1;
            needs[2] = 100;
        }

        SelectTask(102);
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
