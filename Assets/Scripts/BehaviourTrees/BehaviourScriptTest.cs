using BTAI;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;

public class Panda
{

    enum CurrentTask
    {
        None,
        MoveToEat,
        MoveToDrink,
        MoveToShade
    }

    private CurrentTask currentTask = CurrentTask.None;

    private float food = 0;
    private float water = 0;
    private float heat = 0;
    private string id = "";
    private GameObject foodSource, waterSource, shadeSource;

    public Panda(string _id, float _food, float _water, float _heat)
    {
        id = _id;
        food = _food;
        water = _water;
        heat = _heat;
    }

    private void Move(GameObject seek)
    {
        if (MoveState)
    }
    
    private void Eat()
    {
        if ()
    }

    private void Decay()
    {
        food -= 0.5f;
        water -= 1.0f;
        heat += 0.2f;
    }

    private void Update()
    {

    }

    public bool IsNotBusy()
    {
        if (currentTask == CurrentTask.None)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

public class BehaviourScriptTest : MonoBehaviour
{

    Root tree;

    public GameObject agent;

    public GameObject foodSource;
    public GameObject waterSource;
    public GameObject ShadeSource;

    Panda panda = new Panda("Panda1", 100, 100, 100);

    // Start is called before the first frame update
    void Start()
    {
        tree = BT.Root();
        tree.OpenBranch(
            BT.While(() => panda.IsNotBusy()).OpenBranch(
                BT.Selector(false).OpenBranch(
                    BT.Call()

            ),
        );
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
