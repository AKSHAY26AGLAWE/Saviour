using UnityEngine;
using UnityEngine.SceneManagement;  // For scene loading

public class BossDefeatManager : MonoBehaviour
{
    public Animator victoryAnimator;  // Reference to the Animator for the victory animation
    public string nextLevelSceneName = "Level2";  // Name of the next level scene
    public float delayBeforeTransition = 5f;  // Delay before transitioning to the next level

    // This method will be called when the boss is defeated
    public void OnBossDefeated()
    {
        // Play the victory animation
        if (victoryAnimator != null)
        {
            victoryAnimator.SetTrigger("Victory");  // Trigger the victory animation
        }

        // Start the level transition after a delay
        Invoke("TransitionToNextLevel", delayBeforeTransition);
    }

    // This method will handle the transition to the next level
    void TransitionToNextLevel()
    {
        // Load the next level
        SceneManager.LoadScene(nextLevelSceneName);
    }
}
