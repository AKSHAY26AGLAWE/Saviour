using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class PlayerController3D : MonoBehaviour
{
    public GameObject Projectile;
    public Transform shootPoint;
    public float bulletSpeed = 20f;
    public Camera playerCamera;

    public int maxHealth = 3;
    public int currentHealth;
    public TextMeshProUGUI healthText;  // Reference to the TextMeshPro component
    

    public float rotationSpeed = 5f;
    public float maxRotationAngleX = 5f;
    public float maxRotationAngleY = 15f;
    public float returnDelay = 1f;

    private Quaternion targetRotation;
    private Quaternion defaultRotation;
    private bool isAiming = false;
    private bool isReturning = false;

    public GameObject gameOverScreen;

    public Transform[] waypoints;  // Array of waypoints
    private int currentWaypointIndex = 0;
    public float moveSpeed = 5f;

    private Animator animator;
    private GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.Instance;
        currentHealth = maxHealth;
        UpdateHealthDisplay();
        defaultRotation = transform.rotation;
        targetRotation = defaultRotation;
        InitializePlayerState();
        ResetPlayerState(); // Ensure player starts with full health
        animator = GetComponent<Animator>();
        Application.targetFrameRate = 60;

        // Start walking towards the first waypoint
        if (waypoints.Length > 0)
        {
            MoveToNextWaypoint();
        }
    }

    public void UpdateHealthDisplay()
    {
        if (healthText != null)
        {
            healthText.text = currentHealth.ToString();
        }
    }
    public void SetHealth(int health)
    {
        currentHealth = health;
        UpdateHealthDisplay();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ShootAtTarget();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StartCoroutine(ReturnToDefaultPositionAfterDelay(returnDelay));
        }

        if (isAiming || isReturning)
        {
            SmoothRotateGun();
        }

        MoveTowardsWaypoint();
    }

    void MoveTowardsWaypoint()
{
    if (waypoints.Length == 0 || currentWaypointIndex >= waypoints.Length)
        return;

    Transform targetWaypoint = waypoints[currentWaypointIndex];
    Vector3 direction = (targetWaypoint.position - transform.position).normalized;
    float step = moveSpeed * Time.deltaTime;
    transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, step);

    if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
    {
        currentWaypointIndex++;

        if (currentWaypointIndex >= waypoints.Length)
        {
            // Stop the walk animation if the parameter name is correct
            if (animator != null)
            {
                animator.SetBool("isWalking", false);
                Debug.Log("Reached last waypoint, stopping walk animation.");
            }
        }
        else
        {
            MoveToNextWaypoint();
        }
    }
}


    void MoveToNextWaypoint()
    {
        if (currentWaypointIndex < waypoints.Length)
        {
            animator.SetBool("isWalking", true);
        }
    }

    void ShootAtTarget()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            RotateGunTowards(hit.point);

            GameObject bullet = Instantiate(Projectile, shootPoint.position, Quaternion.identity);
            Vector3 direction = (hit.point - shootPoint.position).normalized;
            bullet.GetComponent<Rigidbody>().velocity = direction * bulletSpeed;

            bullet.transform.LookAt(hit.point);
        }
    }

    void RotateGunTowards(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - shootPoint.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.up);
        lookRotation = ClampRotation(lookRotation);
        targetRotation = lookRotation;

        isAiming = true;
        isReturning = false;
    }

    void SmoothRotateGun()
    {
        if (isAiming)
        {
            if (Quaternion.Angle(transform.rotation, targetRotation) > 0.01f)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            }
            else
            {
                transform.rotation = targetRotation;
                isAiming = false;
                StartCoroutine(ReturnToDefaultPositionAfterDelay(returnDelay));
            }
        }
        else if (isReturning)
        {
            if (Quaternion.Angle(transform.rotation, defaultRotation) > 0.01f)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, defaultRotation, Time.deltaTime * rotationSpeed);
            }
            else
            {
                transform.rotation = defaultRotation;
                isReturning = false;
            }
        }
    }

    Quaternion ClampRotation(Quaternion rotation)
    {
        Vector3 euler = rotation.eulerAngles;
        if (euler.x > 180) euler.x -= 360;
        if (euler.y > 180) euler.y -= 360;
        euler.x = Mathf.Clamp(euler.x, -maxRotationAngleX, maxRotationAngleX);
        euler.y = Mathf.Clamp(euler.y, -maxRotationAngleY, maxRotationAngleY);
        return Quaternion.Euler(euler);
    }

    public void TakeDamage()
    {
        currentHealth--;
        Debug.Log("Player hit! Current health: " + currentHealth);        


        if (currentHealth <= 0)
        {
            GameOver();
        }
        UpdateHealthDisplay();
    }

    private void GameOver()
    {
        Debug.Log("Game Over called.");

        // Show the Game Over screen directly
        ShowGameOverScreen();

        // Optionally call GameManager's GameOver method if needed
        if (GameManager.Instance != null)
        {
            GameManager.Instance.GameOver();
        }

        // Stop the game
        Time.timeScale = 0f;

        // Destroy the player object after a short delay to ensure Game Over screen is shown
        StartCoroutine(DestroyPlayerAfterDelay(0.5f));
    }

    public void ShowGameOverScreen()
    {
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);
        }
        else
        {
            Debug.LogError("GameOverScreen reference is missing in PlayerController3D.");
        }
    }

    IEnumerator DestroyPlayerAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyWeapon"))
        {
            TakeDamage();
        }
    }

    IEnumerator ReturnToDefaultPositionAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isReturning = true;
    }

    public void RestartGame()
    {
        // Reset the time scale to normal speed
        Time.timeScale = 1f;

        // Reinitialize player state
        InitializePlayerState();

        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void InitializePlayerState()
    {
        currentHealth = maxHealth;
        if (gameOverScreen != null) gameOverScreen.SetActive(false);
        Time.timeScale = 1f;
        Debug.Log("Game initialized, Time.timeScale: " + Time.timeScale);
    }

    public void ResetPlayerState()
    {
        currentHealth = maxHealth; // Reset health to max
        Debug.Log("Player health reset. Current health: " + currentHealth);
    }
}
