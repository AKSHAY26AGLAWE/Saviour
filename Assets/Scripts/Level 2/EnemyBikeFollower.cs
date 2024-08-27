using UnityEngine;

public class BikeFollower : MonoBehaviour
{
    public Transform targetBike;  // The main bike to follow
    public float baseSpeed = 5f;  // Base speed of the follower bikes
    public float speedVariation = 2f;  // Amount of speed variation
    public Vector3 manualRotation = Vector3.zero; // Manual rotation for the bikes

    private float currentSpeed;

    void Start()
    {
        // Set an initial speed with some variation
        currentSpeed = baseSpeed + Random.Range(-speedVariation, speedVariation);

        // Apply manual rotation
        transform.eulerAngles = manualRotation;
    }

    void Update()
    {
        if (targetBike == null) return;

        // Move towards the target bike
        Vector3 direction = (targetBike.position - transform.position).normalized;
        transform.position += direction * currentSpeed * Time.deltaTime;

        // Rotate to face the target bike
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * currentSpeed);
    }
}
