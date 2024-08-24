using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifeTime = 5f;
    public float speed = 10f; // Time in seconds before the projectile is destroyed

    public int damage = 1;  // Set the amount of damage the projectile does

    private Rigidbody  rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Ensure the projectile has a Rigidbody component
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        // Apply initial force to the projectile
        rb.velocity = transform.forward * speed;

        // Destroy the projectile after 'lifeTime' seconds
        Destroy(gameObject, lifeTime);
    }



    private void OnCollisionEnter(Collision collision)
    {
        TargetScript target = collision.gameObject.GetComponent<TargetScript>();
        if (target != null)
        {
            target.HitByProjectile();
        }
        Destroy(gameObject); // Destroy the projectile after it hits the target

         {
        BossTarget boss = collision.gameObject.GetComponent<BossTarget>();

        if (boss != null)
        {
            // Apply damage to the boss
            boss.TakeDamage(damage);

            // Destroy the projectile after it hits the boss
            Destroy(gameObject);
        }
        else
        {
            // Destroy the projectile on any other collision
            Destroy(gameObject);
        }
    }
    }
}
