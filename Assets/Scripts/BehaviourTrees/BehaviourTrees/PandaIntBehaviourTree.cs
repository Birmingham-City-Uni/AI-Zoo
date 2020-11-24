using BTAI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.UI;

public class PandaIntBehaviourTree : MonoBehaviour
{
    // Start Tree node
    Root tree;

    // Game objects used to specify sourcers
    public GameObject foodSource;
    public GameObject waterSource;
    public GameObject shelterSource;

    public Text currentTask;
    public Slider awakeness;
    public Slider water;
    public Slider food;

    // Panda animator
    private Animator anim;

    MovementScript moveScript;

    // Panda object
    PandaIntelligent panda;

    // Speed in which panda moves toward sources
    public float pandaSpeed;

    // Start is called before the first frame update
    void Start()
    {

        anim = GetComponent<Animator>();
        moveScript = GetComponent<MovementScript>();

        // Create panda with following parameters
        panda = new PandaIntelligent("Intelligent", this.gameObject, 80, 80, 80, pandaSpeed, anim, moveScript);

#pragma warning disable format
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
#pragma warning restore format 
    }

    // Update is called once per frame
    void Update()
    {
        // Calls panda class update method
        panda.Update();
        // Calls behaviour tree updater
        tree.Tick();

        awakeness.value = panda.awakeness / 100;
        water.value = panda.water / 100;
        food.value = panda.food / 100;

        switch (panda.task)
        {
            case Panda.Target.food:
                currentTask.text = "Eating";
                break;
            case Panda.Target.water:
                currentTask.text = "Drinking";
                break;
            case Panda.Target.shelter:
                currentTask.text = "Sleeping";
                break;
        }
    }
}
