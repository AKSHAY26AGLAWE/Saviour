using UnityEngine;
using UnityEngine.SceneManagement;

public class LandingPageController : MonoBehaviour
{
    public GameObject landingPagePanel; // Reference to the Landing Page panel

    void Start()
    {
        // Ensure the landing page is active at the start
        landingPagePanel.SetActive(true);
        Time.timeScale = 0f; // Pause the game initially
    }

    public void OnPlayButtonClicked()
{
    Debug.Log("Tap to Play button clicked. Resuming game.");
    landingPagePanel.SetActive(false);
    Time.timeScale = 1f; // Resume the game
    Debug.Log("Time.timeScale after Tap to Play: " + Time.timeScale);
}

}
