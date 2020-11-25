using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITowardsPlayerScript : MonoBehaviour
{
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        // Get player object
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // Set UI rotation to the rotation vector between UI and player position
        transform.rotation = Quaternion.LookRotation(this.transform.position - player.transform.position);
    }
}
