using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject gameOverScreen; // Reference to the Game Over UI

    private bool isGameOver;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("GameManager instance set.");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // This method resets the game state when retrying
    public void ResetGameState()
    {
        isGameOver = false; // Reset Game Over state
        // Reset other necessary game states here
    }

    public void GameOver()
    {
        if (isGameOver) return; // Prevent calling GameOver multiple times
        isGameOver = true; // Mark the game as over
        Debug.Log("Player health is 0, triggering Game Over");
        StartCoroutine(HandleGameOver());
    }

    // Stops all coroutines and resets necessary game elements
    public void StopAllCoroutinesAndListeners()
    {
        StopAllCoroutines();  // Stop all coroutines
        // Unsubscribe from events if necessary
    }

    public void RetryGame()
{
    StopAllCoroutinesAndListeners();  // Stop ongoing processes
    ResetGameState();           // Reset game states
    Time.timeScale = 1f;        // Ensure the game runs at normal speed
    
    GameObject player = GameObject.FindGameObjectWithTag("Player");
    if (player != null)
    {
        PlayerController3D playerController = player.GetComponent<PlayerController3D>();
        if (playerController != null)
        {
            playerController.ResetPlayerState(); // Reset player's health
        }
    }
    SceneManager.LoadScene("Paint Assassin"); // Reload the game scene
}


    private IEnumerator HandleGameOver()
    {
        // Optionally, add a delay before showing the Game Over screen
        yield return new WaitForSeconds(1f);
        
        // Show the Game Over screen
        ShowGameOverScreen();
    }

    public void ShowGameOverScreen()
    {
        if (gameOverScreen != null)
        {
            Debug.Log("Attempting to activate Game Over screen...");
            gameOverScreen.SetActive(true); // Activate the Game Over screen
            Debug.Log("Game Over screen should be active now.");
        }
        else
        {
            Debug.LogError("GameOverScreen reference is missing.");
        }

        Time.timeScale = 0f; // Pause the game
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
