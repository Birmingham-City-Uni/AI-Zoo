using BTAI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class PandaIntBehaviourTree : MonoBehaviour
{
    // Start Tree node
    Root tree;

    private NavMeshAgent agent;

    // Game objects used to specify sourcers
    public GameObject foodSource;
    public GameObject waterSource;
    public GameObject shelterSource;

    // UI elements used for floating GUI
    public Text currentTask;
    public Slider awakeness;
    public Slider water;
    public Slider food;

    // Panda animator
    private Animator anim;

    // Panda object
    PandaIntelligent panda;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        // Create panda with following parameters
        panda = new PandaIntelligent("Intelligent", this.gameObject, 80, 80, 80, anim, agent);

        // Used to stop the linter giving whitespace errors
#pragma warning disable format
        // Create root node
        tree = BT.Root();
        // Open initial branch
        tree.OpenBranch(
            // Checks if panda is currently performing a task
            BT.While(() => panda.IsNotBusy()).OpenBranch(
                BT.Selector(true).OpenBranch(
                // Uses weighting to decide a task for the panda to procede with based upon the pandas need
                BT.If(() => panda.PandaShouldEat()).OpenBranch(
                    // Sets the pandas task giving the source of that task
                    BT.Call(() => panda.SetTask(foodSource, Panda.Target.food))),
                BT.If(() => panda.PandaShouldDrink()).OpenBranch(
                    BT.Call(() => panda.SetTask(waterSource, Panda.Target.water))),
                BT.If(() => panda.PandaShouldSleep()).OpenBranch(
                    BT.Call(() => panda.SetTask(shelterSource, Panda.Target.shelter)))
                )
            )
        ) ; ; ;
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
