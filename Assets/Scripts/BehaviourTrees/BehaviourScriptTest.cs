using BTAI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Profiling;
using UnityEngine;

public class Panda
{

    private float food = 0;
    private float water = 0;
    private float temperature = 0;
    private float speed = 0;
    private string id = "";

    MovementScript moveScript = new MovementScript();
    GameObject panda;

    public enum Target
    {
        unitialised,
        food,
        water,
        shelter
    }

    private Target task = Target.unitialised;
    private GameObject target;

    public Panda(string _id, GameObject _panda, float _food, float _water, float _temperature, float _speed)
    {
        id = _id;
        panda = _panda;
        food = _food;
        water = _water;
        temperature = _temperature;
        speed = _speed;
    }

    public void SetTask(GameObject _target, Target _task)
    {

        Debug.Log("Task set " + _task);
        target = _target;
        task = _task;
    }

    private void Move()
    {
        // Get distance and angle

        float angle_to_turn = moveScript.CalculateAngle(panda.gameObject, target.transform.position);

        panda.transform.Rotate(0, angle_to_turn * Time.deltaTime * 10.0f, 0);

        // Translate locally forward in z
        panda.transform.Translate(new Vector3(0, 0, speed * Time.deltaTime), Space.Self);
    }

    private void Decay()
    {
        food -= 0.5f * Time.deltaTime;
        water -= 1.0f * Time.deltaTime;
        temperature -= 0.2f * Time.deltaTime;
    }

    public void Update()
    {
        Decay();

        if (task != Target.unitialised)
        {
            if (moveScript.CalculateDistance(panda.gameObject, target.transform.position) > 0.5f)
            {
                Move();
            }
            else
            {
                if (task == Target.food)
                {
                    if (food < 99.0f)
                    {
                        food += 0.2f;
                    }
                    else
                    {
                        task = Target.unitialised;
                    }
                }
                else if (task == Target.water)
                {
                    if (water < 99.0f)
                    {
                        water += 0.2f;
                    }
                    else
                    {
                        task = Target.unitialised;
                    }
                }
                else
                {
                    if (temperature < 100.0f)
                    {
                        temperature += 0.2f;
                    }
                    else
                    {
                        task = Target.unitialised;
                    }
                }
            }
        }
    }

    public int[] CheckNeed()
    {
        // Food index 0,  Water index 1, shade index 3
        int[] needs = new int[3];

        if (food >= water)
        {
            if (food >= temperature)
            {
                // food > heat > water
                needs[0] = 1;
                needs[1] = 50;
                needs[2] = 100;
            }
            else
            {
                // heat > food > water
                needs[0] = 50;
                needs[1] = 100;
                needs[2] = 1;
            }
        }
        else
        {
            if (water >= temperature)
            {
                // water > heat > food
                needs[0] = 100;
                needs[1] = 1;
                needs[2] = 50;
            }
            else
            {
                // heat > water > food
                needs[0] = 100;
                needs[1] = 50;
                needs[2] = 1;
            }
        }

        return needs;
    }

    public bool IsNotBusy()
    {
        if (task == Target.unitialised)
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

    public GameObject foodSource;
    public GameObject waterSource;
    public GameObject shelterSource;

    Panda panda;

    public float pandaSpeed;

    // Start is called before the first frame update
    void Start()
    {
        panda = new Panda("Panda1", this.gameObject, 100, 100, 100, pandaSpeed);

        tree = BT.Root();
        tree.OpenBranch(
            BT.While(() => panda.IsNotBusy()).OpenBranch(
                BT.RandomSequence(panda.CheckNeed()).OpenBranch(
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
        panda.Update();
        tree.Tick();
    }
}
