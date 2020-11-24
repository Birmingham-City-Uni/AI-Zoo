using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PandaSemiIntelligent : Panda
{
    public PandaSemiIntelligent(string _id, GameObject _panda, float _food, float _water, float _awakeness, float _speed, Animator _anim, MovementScript _moveScript)
    : base(_id, _panda, _food, _water, _awakeness, _speed, _anim, _moveScript)
    {

    }

    public override int[] CheckNeed()
    {
        // Array for each need
        // Food index 0,  Water index 1, heat index 3
        int[] needs = new int[3];

        // Checks what animal currently needs more
        for (int i = 0; i < needs.Length; i++)
        {
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
        }
        return needs;
    }
}
