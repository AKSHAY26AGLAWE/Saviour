using UnityEngine;
using System.Collections;

public class TargetScript : MonoBehaviour
{
    public int hitsToDestroy = 2; // Targets will be destroyed after 2 hits
    private int hitCount = 0;

    public float speed = 2f;
    public float rotationSpeed = 3f;
    public float attackRange = 1f;

    private Animator targetAnimator;
    private bool isAttacking = false;
    private bool isDead = false;
    private GameObject player;

    public float separationDistance = 1f; // Minimum distance between targets
    public float avoidanceStrength = 5f; // Strength of avoidance force
    public float minimumApproachDistance = 3f; // Minimum distance at which the target will approach the player
    public float triggerDistance = 20f; // Distance at which the target starts moving towards the player

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        targetAnimator = GetComponent<Animator>();
        if (targetAnimator == null)
        {
            Debug.LogError("Target Animator component not found on the target.");
        }
    }

    void Update()
    {
        if (isDead) return; // Stop all movement and rotation if the target is dead

        if (!isAttacking)
        {
            AvoidOtherTargets();
            MoveTowardsPlayer();
        }

        if (player == null) return;

        // Always update the direction towards the player
        Vector3 direction = (player.transform.position - transform.position).normalized;

        // Smoothly rotate towards the player
        RotateTowardsPlayer(direction);
    }

    void MoveTowardsPlayer()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        // Check if the player is within the trigger distance
        if (distanceToPlayer <= triggerDistance)
        {
            // Only move towards the player if they are beyond the minimum approach distance
            if (distanceToPlayer > minimumApproachDistance)
            {
                // Move towards the player only if they are outside the attack range
                if (distanceToPlayer > attackRange)
                {
                    // Move towards the player at normal speed
                    transform.position += transform.forward * speed * Time.deltaTime;
                    if (isAttacking) 
                    {
                        StopCoroutine("PrepareAttack");
                        targetAnimator.SetBool("isAttacking", false);
                        isAttacking = false;
                    }
                }
                else if (!isAttacking)
                {
                    // Move slowly towards the player and prepare to attack
                    transform.position += transform.forward * (speed * 0.5f) * Time.deltaTime; // Reduce speed by half during attack
                    StartCoroutine(PrepareAttack());
                }
            }
            else if (!isAttacking)
            {
                // Player is within the minimum approach distance, so the target remains stationary and prepares to attack
                StartCoroutine(PrepareAttack());
            }
        }
    }

    private IEnumerator PrepareAttack()
    {
        isAttacking = true;
        yield return new WaitForSeconds(0.5f); // Small delay before attack animation
        targetAnimator.SetBool("isAttacking", true);
    }

    void RotateTowardsPlayer(Vector3 direction)
    {
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
    }

    public void HitByProjectile()
    {
        if (isDead) return; // Do nothing if already dead

        hitCount++;
        Debug.Log("Hit count: " + hitCount); // Debugging hit count

        if (hitCount >= hitsToDestroy)
        {
            StartCoroutine(Die()); // Start the Die coroutine to handle the death process
        }
    }

    private IEnumerator Die()
    {
        isDead = true; // Mark the target as dead to prevent further actions
        targetAnimator.SetTrigger("Die"); // Trigger the death animation

        // Wait until the death animation finishes
        yield return new WaitForSeconds(targetAnimator.GetCurrentAnimatorStateInfo(0).length);

        // Additional delay before destroying the target
        yield return new WaitForSeconds(1f); // Adjust this value for your desired delay

        DestroyTarget(); // Destroy the target after the delay
    }

    private void DestroyTarget()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isDead) return; // Do nothing if already dead

        if (collision.gameObject.CompareTag("Player"))
        {
            // Start the attack animation and stop moving
            isAttacking = true;
            targetAnimator.SetBool("isAttacking", true);

            // Optionally, deal damage to the player
            PlayerScript playerScript = collision.gameObject.GetComponent<PlayerScript>();
            if (playerScript != null)
            {
                playerScript.TakeDamage(1);
            }
        }
    }

    void AvoidOtherTargets()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Target");

        foreach (GameObject otherTarget in targets)
        {
            if (otherTarget != gameObject)
            {
                float distance = Vector3.Distance(transform.position, otherTarget.transform.position);

                if (distance < separationDistance)
                {
                    Vector3 directionAway = transform.position - otherTarget.transform.position;
                    transform.position += directionAway.normalized * avoidanceStrength * Time.deltaTime;
                }
            }
        }
    }
}
