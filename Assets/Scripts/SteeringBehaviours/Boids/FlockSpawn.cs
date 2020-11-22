using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FlockSpawn : MonoBehaviour
{

    // Bird object
    public GameObject birdPrefab;

    // Size of bounding box that dictates both boid spawn position, and movement bounds
    public static int flockBounds = 100;
    // Amount of boids that are spawned
    private static int flockSize = 700;

    public static GameObject[] birdsArray = new GameObject[flockSize];

    public GameObject goalPositionParentNode;

    private GameObject[] goalPositionsArray;

    public static Vector3 currentGoalPosition;
    private Vector3 birdSpawnPosition;
    private int currentGoalIndex;

    public GameObject player;

    private Vector3 playerPosition;

    // Start is called before the first frame update
    void Start()
    {
        int arraySize = goalPositionParentNode.transform.childCount;
        goalPositionsArray = new GameObject[arraySize];

        for (int i = 0;  i < arraySize;  i++)
        {
            goalPositionsArray[i] = goalPositionParentNode.transform.GetChild(i).gameObject;
        }

        currentGoalIndex = Random.Range(0, arraySize);
        playerPosition = player.transform.position;
        currentGoalPosition = goalPositionsArray[currentGoalIndex].transform.position;

        SpawnBirds();
    }

    private void Update()
    {
        // Update player position to keep gizmos updating
        playerPosition = player.transform.position;
    }

    void SpawnBirds()
    {
        for (int i = 0; i < flockSize; i++)
        {
            birdSpawnPosition = new Vector3(Random.Range(playerPosition.x - flockBounds, playerPosition.x + flockBounds),
                                            Random.Range(playerPosition.y + 10f, playerPosition.y + flockBounds),
                                            Random.Range(playerPosition.z - flockBounds, playerPosition.z + flockBounds));
            // Create birds at random position within bounds with differing rotations
            birdsArray[i] = Instantiate(birdPrefab, birdSpawnPosition, Quaternion.LookRotation(new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f))));
        }
    }

    void OnDrawGizmos()
    {
        // Display boid bounds in the scene
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(new Vector3(playerPosition.x, playerPosition.y + 55, playerPosition.z), new Vector3(flockBounds * 2, 90, flockBounds * 2));
    }
}
