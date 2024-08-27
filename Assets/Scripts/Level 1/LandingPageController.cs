using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LandingPageController : MonoBehaviour
{
    public GameObject landingPagePanel; 
    public Animator tapToPlayAnimator;
    public GameObject tapToPlayPanel;

    public Button[] buttonsToDisable;
    public GameObject progressBar;
    public Button[] buttonsToEnable;
    public GameObject otherUIElement;

    void Start()
    {
        // Ensure the landing page is active at the start
        landingPagePanel.SetActive(true);
        Time.timeScale = 0f; // Pause the game initially
        tapToPlayAnimator.SetBool("DefaultAnim", true);
    }
    public void OnPlayButtonClicked()
    {
        Debug.Log("Tap to Play button clicked. Resuming game.");
        landingPagePanel.SetActive(false);
        Time.timeScale = 1f; // Resume the game
        Debug.Log("Time.timeScale after Tap to Play: " + Time.timeScale);
        //tapToPlayAnimator.SetBool("DefaultAnim", false);
        DisableButtons();
        EnableGameObjects();
    }

    public void DisableButtons()
    {
        foreach (Button button in buttonsToDisable)
        {
            button.gameObject.SetActive(false); // Deactivates the button GameObjects
        }
    }
    public void EnableGameObjects()
    {
        if (progressBar != null)
        {
            progressBar.SetActive(true);  // Activate the progress bar
        }
        foreach (Button button in buttonsToEnable)
        {
            if (button != null)
            {
                button.gameObject.SetActive(true);  // Activate each specified button
            }
        }
        if (otherUIElement != null)
        {
            otherUIElement.SetActive(true);  // Activate any other UI elements
        }
    }
}