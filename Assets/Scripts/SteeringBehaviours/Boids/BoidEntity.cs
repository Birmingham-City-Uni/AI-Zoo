using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidEntity : MonoBehaviour
{
    // Player object and position
    private GameObject player;
    private Vector3 playerPosition;

    // Boid rotation and speed
    private float rotationSpeed = 3.0f;
    private float speed = 1.0f;

    // Distance in which a boid considers another boid a neighbour
    private float distanceToNeighbour = 10.0f;

    // Set as true when a bird needs to turn in order to stay in the bounds
    private bool turning = true;

    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        // Sets a random speed so all boids are not uniform in speed
        speed = Random.Range(0.5f, 2.0f);
        // Finds player in the scene
        player = GameObject.FindWithTag("Player");

        // Gets starting position
        playerPosition = player.transform.position;

        // Gets animaton component
        // Plays flying animation at 1/5 the speed
        anim = GetComponent<Animator>();
        anim.speed = 0.2f;
        anim.Play("Fly");
    }

    // Update is called once per frame
    void Update()
    {
        // Update player position so that boids goal moves with player
        playerPosition = player.transform.position;

        // Checks if bird is out of the bounding box distance
        if (Vector3.Distance(this.transform.position, playerPosition) >= FlockSpawn.flockBounds)
        {
            turning = true;
        }
        // Checks if bird is too low to the ground
        else if (this.transform.position.y <= 10.0f)
        {
            turning = true;
        }
        // Else, bird does not need to turn
        else
        {
            turning = false;
        }

        // If turn is true. Face the bird towards the player at a random position above it
        if (turning == true)
        {
            // Create random vector above the player
            Vector3 direction = playerPosition + new Vector3(0, Random.Range(10f, FlockSpawn.flockBounds), 0) - this.transform.position;
            // Rotates the boid towards that new direction
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
            // Gives a random speed
            speed = Random.Range(0.5f, 3.0f);
        }

        // Only evaluates true when bird isnt turning back into the bounds
        // Only enters 20% of the time
        if (Random.Range(0, 5) < 1 && turning != true)
        {
            // Sets up an array of birds
            GameObject[] flockBirds;
            // Gets static bird array from FlockSpawn
            flockBirds = FlockSpawn.birdsArray;

            // Create placeholder positions
            Vector3 centerPosition = this.transform.position;
            Vector3 avoidPosition = playerPosition;

            // Speed of the flock
            float flockSpeed = 0.5f;
            // Goal position that the birds want to flock to
            Vector3 goalPos = FlockSpawn.currentGoalPosition;

            // Distance between boids
            float distance;
            int neighbourGroupSize = 0;

            // Iterate through birds
            foreach (GameObject bird in flockBirds)
            {
                // Check if not same bird
                if (bird != this.gameObject)
                {
                    // Check if distance between voids is less than 10
                    distance = Vector3.Distance(bird.transform.position, this.transform.position);
                    if (distance <= distanceToNeighbour)
                    {
                        // Add each bird in the flocks position to the center position of the flock
                        centerPosition += bird.transform.position;
                        // Increase the flock size by 1
                        neighbourGroupSize += 1;

                        // Check if distance between birds is too close
                        if (distance < 6.0f)
                        {
                            // Add the vector between this boid and other boid to avoid position
                            avoidPosition += (this.transform.position - bird.transform.position);
                        }

                        // Get the other birds script component
                        BoidEntity AnotherFlockEntity = bird.GetComponent<BoidEntity>();
                        // Interate the flock speed by that birds speed
                        flockSpeed += AnotherFlockEntity.speed;
                    }
                }
            }

            // Check if the bird has any neighbours
            if (neighbourGroupSize > 0)
            {
                // Divide the center position by the group side and the distance between this bird and the goal
                centerPosition = centerPosition / neighbourGroupSize + (goalPos - this.transform.position);

                // Get the relative speed that the bird should travel at
                // With additional random increment/decrement to stop uniform speed occuring
                speed = (flockSpeed / neighbourGroupSize) + Random.Range(-0.1f, 0.1f);
                // Check if speed is too high in case of a large flock of birds
                if (speed > 5.0f)
                {
                    // Set speed to a move realistic number
                    speed = Random.Range(0.5f, 3.0f);
                }

                // Create a new direction from the bird towards the vector inbetween the center and avoid position
                Vector3 direction = (centerPosition + avoidPosition) - this.transform.position;

                // Rotate towards that direction vector rotating at the pre determined rotation speed
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
            }
        }
        // Move the bird in local Z axis
        transform.Translate(0, 0, speed * Time.deltaTime);
    }
}
