using BTAI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Profiling;
using UnityEngine;

public class PandaBehaviourTree : MonoBehaviour
{
    // Start Tree node
    Root tree;

    // Game objects used to specify sourcers
    public GameObject foodSource;
    public GameObject waterSource;
    public GameObject shelterSource;

    // Panda animator
    private Animator anim;

    MovementScript moveScript;

    // Panda object
    Panda panda;

    // Speed in which panda moves toward sources
    public float pandaSpeed;

    // Start is called before the first frame update
    void Start()
    {

        anim = GetComponent<Animator>();
        moveScript = GetComponent <MovementScript>();

        // Create panda with following parameters
        panda = new Panda("Panda1", this.gameObject, 80, 80, 80, pandaSpeed, anim, moveScript);

        // Create root node
        tree = BT.Root();
        // Open initial branch
        tree.OpenBranch(
            // Checks if panda is currently performing a task
            BT.While(() => panda.IsNotBusy()).OpenBranch(
                // Uses weighting to decide a task for the panda to procede with based upon the pandas need
                BT.RandomSequence(panda.CheckNeed()).OpenBranch(
                    // Sets the pandas task giving the source of that task
                    BT.Call(() => panda.SetTask(foodSource, Panda.Target.food)),
                    BT.Call(() => panda.SetTask(waterSource, Panda.Target.water)),
                    BT.Call(() => panda.SetTask(shelterSource, Panda.Target.shelter))
                )
            )
        );
    }

    // Update is called once per frame
    void Update()
    {
        // Calls panda class update method
        panda.Update();
        // Calls behaviour tree updater
        tree.Tick();
    }
}
