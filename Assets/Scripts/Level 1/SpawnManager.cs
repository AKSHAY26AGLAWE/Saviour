using UnityEngine;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] bikePrefabs; // Array of bike prefabs to spawn
    public Transform[] spawnPoints;  // Array of spawn points
    public float spawnInterval = 5f; // Time interval between spawns
    public int maxBikes = 10;        // Maximum number of bikes to spawn
    public float minDistance = 5f;   // Minimum distance between bikes

    private List<GameObject> spawnedBikes = new List<GameObject>();
    private int currentBikeCount = 0;

    void Start()
    {
        // Start spawning bikes at regular intervals
        InvokeRepeating("SpawnBike", 0f, spawnInterval);
    }

    void SpawnBike()
    {
        if (currentBikeCount >= maxBikes) return;

        // Try to spawn at a valid point
        bool spawned = false;
        int attempts = 0;

        while (!spawned && attempts < 10)
        {
            attempts++;

            // Select a random bike prefab
            int prefabIndex = Random.Range(0, bikePrefabs.Length);
            GameObject selectedBikePrefab = bikePrefabs[prefabIndex];

            // Select a random spawn point
            int spawnIndex = Random.Range(0, spawnPoints.Length);
            Transform spawnPoint = spawnPoints[spawnIndex];

            // Check if the spawn point is far enough from existing bikes
            bool canSpawn = true;
            foreach (GameObject bike in spawnedBikes)
            {
                if (Vector3.Distance(spawnPoint.position, bike.transform.position) < minDistance)
                {
                    canSpawn = false;
                    break;
                }
            }

            // If the spawn point is valid, spawn the bike
            if (canSpawn)
            {
                GameObject newBike = Instantiate(selectedBikePrefab, spawnPoint.position, spawnPoint.rotation);
                spawnedBikes.Add(newBike);
                currentBikeCount++;
                spawned = true;
            }
        }
    }
}
