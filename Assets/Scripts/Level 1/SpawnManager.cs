using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject[] colorprefabs;
  
    private float startDelay = 2;
    private float spawnInterval = 5f;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnRandomColor", startDelay, spawnInterval);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnRandomColor()
    {
        int colorIndex = Random.Range(0, colorprefabs.Length);
        Vector3 spawnPos = new Vector3(Random.Range (5.91f, 13.2f), 4.29f, -1330.3f);
        Instantiate(colorprefabs[colorIndex], spawnPos, colorprefabs[colorIndex].transform.rotation);
    }
    
}
