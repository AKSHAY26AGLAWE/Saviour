using UnityEngine;
using System.Collections;

public class BossTarget : MonoBehaviour
{
    public GameObject axePrefab; // Prefab of the axe to throw
    public Transform axeSpawnPoint; // Point from where the axe will be thrown
    public float throwInterval = 2f; // Interval between throws
    public float throwForce = 10f; // Force with which the axe is thrown
    public float triggerDistance = 10f; // Distance at which the boss starts throwing axes

    private GameObject player;
    private bool isThrowing = false;
    public int maxHealth = 5;  // Set the boss's maximum health
    private int currentHealth;

    private Animator bossAnimator;
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;  // Initialize current health
        bossAnimator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogError("Player not found in the scene.");
        }
    }

    // Call this method when the boss is hit by a projectile
    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            StartCoroutine(Die());
        }
    }

    private IEnumerator Die()
    {
        isDead = true;
        bossAnimator.SetTrigger("Die");

        // Wait for the die animation to finish
        float dieAnimationLength = bossAnimator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(dieAnimationLength);

        // Destroy the boss after the animation is finished
        Destroy(gameObject);
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        // Start throwing axes if the player is within the trigger distance
        if (distanceToPlayer <= triggerDistance && !isThrowing)
        {
            isThrowing = true;
            StartCoroutine(ThrowAxes());
        }
        // Stop throwing axes if the player moves out of range
        else if (distanceToPlayer > triggerDistance && isThrowing)
        {
            isThrowing = false;
            StopCoroutine(ThrowAxes());
        }
    }

    private IEnumerator ThrowAxes()
    {
        while (true)
        {
            ThrowAxeAtPlayer();
            yield return new WaitForSeconds(throwInterval);
        }
    }

    private void ThrowAxeAtPlayer()
    {
        if (player == null) return;

        // Instantiate the axe at the spawn point
        GameObject axe = Instantiate(axePrefab, axeSpawnPoint.position, axeSpawnPoint.rotation);

        // Calculate direction towards the player
        Vector3 directionToPlayer = (player.transform.position - axeSpawnPoint.position).normalized;

        // Apply force to the axe in the direction of the player
        axe.GetComponent<Rigidbody>().AddForce(directionToPlayer * throwForce, ForceMode.VelocityChange);
    }
}
