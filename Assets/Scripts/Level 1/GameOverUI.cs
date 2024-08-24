using UnityEngine;
using UnityEngine.SceneManagement; // Make sure this line is included

public class GameOverUI : MonoBehaviour
{
    public void OnRetryButtonClick()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RetryGame();

            SceneManager.LoadScene("Paint Assassin");
        }
    }
}
