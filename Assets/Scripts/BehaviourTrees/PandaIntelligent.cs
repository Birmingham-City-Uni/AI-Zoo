using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PandaIntelligent : Panda
{

    public PandaIntelligent(string _id, GameObject _panda, float _food, float _water, float _awakeness, float _speed, Animator _anim, MovementScript _moveScript) 
        : base(_id, _panda, _food, _water, _awakeness, _speed, _anim, _moveScript)
    {

    }

    public override int[] CheckNeed()
    {
        // Array for each need
        // Food index 0,  Water index 1, heat index 3
        int[] needs = new int[3];

        // Gives weighting to most important task
        if (food < water && food < awakeness)
        {
            needs[0] = 100;
            needs[1] = 0;
            needs[2] = 0;
        }
        else if (water < food && water < awakeness)
        {
            needs[0] = 0;
            needs[1] = 100;
            needs[2] = 0;
        }
        else
        {
            needs[0] = 0;
            needs[1] = 0;
            needs[2] = 100;
        }

        return needs;
    }
}
