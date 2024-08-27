using UnityEngine;

public class BikeFollower : MonoBehaviour
{
    public Transform targetBike; // Reference to the main bike
    public float speedVariation = 1.0f; // Speed variation for each bike
    public Vector3 positionOffset; // Manual position offset
    public Vector3 rotationOffset; // Manual rotation offset

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    void Start()
    {
        // Store the initial position and rotation
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    void Update()
    {
        // Follow the target bike while maintaining manual offset
        Vector3 targetPosition = targetBike.position + positionOffset;
        Quaternion targetRotation = targetBike.rotation * Quaternion.Euler(rotationOffset);

        // Move towards the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * speedVariation);

        // Rotate towards the target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * speedVariation);
    }
}
