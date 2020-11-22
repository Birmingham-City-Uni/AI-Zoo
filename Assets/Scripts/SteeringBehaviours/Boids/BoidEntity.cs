using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidEntity : MonoBehaviour
{

    private GameObject player;

    private Vector3 playerPosition;

    [SerializeField]
    private float rotationSpeed = 3.0f;

    private float speed = 1.0f;

    private float distanceToNeighbour = 10.0f;
    private bool turning = true;

    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(0.5f, 2.0f);
        player = GameObject.FindWithTag("Player");
        playerPosition = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Update player position so that boids goal moves with player
        playerPosition = player.transform.position;

        if (Vector3.Distance(this.transform.position, playerPosition) >= FlockSpawn.flockBounds)
        {
            turning = true;
        }
        else if (this.transform.position.y <= 10.0f)
        {
            turning = true;
        }
        else
        {
            turning = false;
        }

        if (turning == true)
        {
            Vector3 direction = playerPosition + new Vector3(0, Random.Range(10f, FlockSpawn.flockBounds), 0) - this.transform.position;
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
            speed = Random.Range(0.5f, 3.0f);
        }

        if (Random.Range(0,5) < 1  && turning != true)
        {
            GameObject[] flockBirds;
            flockBirds = FlockSpawn.birdsArray;

            Vector3 centerPosition = this.transform.position;
            Vector3 avoidPosition = playerPosition;

            float flockSpeed = 0.5f;
            Vector3 goalPos = FlockSpawn.currentGoalPosition;

            float distance;
            int neighbourGroupSize = 0;

            foreach (GameObject bird in flockBirds)
            {
                if (bird != this.gameObject)
                {
                    distance = Vector3.Distance(bird.transform.position, this.transform.position);
                    if (distance <= distanceToNeighbour)
                    {
                        centerPosition += bird.transform.position;
                        neighbourGroupSize += 1;

                        if (distance < 6.0f)
                        {
                            avoidPosition = avoidPosition + (this.transform.position - bird.transform.position);
                        }

                        BoidEntity AnotherFlockEntity = bird.GetComponent<BoidEntity>();
                        flockSpeed += AnotherFlockEntity.speed;
                    }
                }
            }

            if (neighbourGroupSize > 0)
            {
                centerPosition = centerPosition / neighbourGroupSize + (goalPos - this.transform.position);

                speed = (flockSpeed / neighbourGroupSize) + Random.Range(-0.1f, 0.1f);
                if (speed > 5.0f)
                {
                    speed = Random.Range(0.5f, 3.0f);
                }

                Vector3 direction = (centerPosition + avoidPosition) - this.transform.position;

                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
            }
        }
        transform.Translate(0, 0, speed * Time.deltaTime);
    }
}
