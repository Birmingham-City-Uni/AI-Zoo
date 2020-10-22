using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurn : MonoBehaviour
{

    public Transform player;

    public float turnRateSpeed = 1.0f;
    public float lookRateSpeed = 1.0f;

    private float turnRate;
    private float lookRate;

    private bool canLook;

    // Start is called before the first frame update
    void Start()
    {
        canLook = true;
    }

    private void Look()
    {
        turnRate = Input.GetAxis("Mouse X") * turnRateSpeed;
        lookRate = Input.GetAxis("Mouse Y") * lookRateSpeed;

        if (turnRate != 0)
        {
            player.transform.eulerAngles += new Vector3(0.0f, turnRate, 0.0f);
        }
        if (lookRate != 0)
        {
            this.transform.eulerAngles += new Vector3(-lookRate, 0.0f, 0.0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (canLook == true)
        {
            Look();
        }
    }
}
