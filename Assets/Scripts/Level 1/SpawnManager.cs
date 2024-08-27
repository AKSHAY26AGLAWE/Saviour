using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] bikePrefabs; // Array of bike prefabs to spawn
    public Transform[] spawnPoints;  // Array of spawn points
    public float spawnInterval = 5f; // Time interval between spawns
    public int maxBikes = 10;        // Maximum number of bikes to spawn

    private int currentBikeCount = 0;

    void Start()
    {
        // Start spawning bikes at regular intervals
        InvokeRepeating("SpawnBike", 0f, spawnInterval);
    }

    void SpawnBike()
    {
        if (currentBikeCount >= maxBikes) return;

        // Select a random bike prefab
        int prefabIndex = Random.Range(0, bikePrefabs.Length);
        GameObject selectedBikePrefab = bikePrefabs[prefabIndex];

        // Select a random spawn point
        int spawnIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[spawnIndex];

        // Spawn the selected bike at the selected spawn point
        Instantiate(selectedBikePrefab, spawnPoint.position, spawnPoint.rotation);

        currentBikeCount++;
    }
}
