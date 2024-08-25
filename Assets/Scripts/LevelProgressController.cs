using UnityEngine;
using UnityEngine.UI;

public class LevelProgressController : MonoBehaviour
{
    public Slider progressBar;   // Assign the progress bar in the Inspector
    public Transform player;     // Assign the player's Transform in the Inspector
    public Transform startPoint; // Starting point of the level
    public Transform endPoint;   // End of the level

    void Update()
    {
        UpdateProgress();
    }

    void UpdateProgress()
    {
        // Calculate the progress percentage from start to end point
        float totalDistance = Vector3.Distance(startPoint.position, endPoint.position);
        float playerDistance = Vector3.Distance(startPoint.position, player.position);

        // Update the progress bar with the percentage of progress
        progressBar.value = Mathf.Clamp01(playerDistance / totalDistance);

        // Check if the player reached the end
        if (player.position.x >= endPoint.position.x)
        {
            // Game Finished
            Debug.Log("Level Complete!");
            // Trigger any level completion logic here
        }
    }
}
