using UnityEngine;
using System.Collections; // Include this namespace for IEnumerator

public class FollowCamera : MonoBehaviour
{
    public Transform player; // Reference to the player object
    public Vector3 offset = new Vector3(0, 5, -10); // Offset position from the player
    public float smoothSpeed = 0.125f; // Smooth speed of camera movement
    public float transitionDuration = 2f; // Duration of the transition effect

    private Vector3 startPosition;
    private Quaternion initialRotation;
    private bool isTransitioning = true;

    void Start()
    {
        // Save the initial position and rotation of the camera
        startPosition = transform.position;
        initialRotation = transform.rotation;

        // Start the transition to the follow position
        StartCoroutine(TransitionToFollowPosition());
    }

    void LateUpdate()
    {
        if (!isTransitioning && player != null)
        {
            // Calculate the desired position with offset
            Vector3 desiredPosition = player.position + offset;

            // Smoothly move camera to the desired position
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;

            // Keep the initial rotation
            transform.rotation = initialRotation;
        }
    }

    IEnumerator TransitionToFollowPosition()
    {
        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            // Calculate the transition progress
            float t = elapsedTime / transitionDuration;

            // Calculate the desired position with offset
            Vector3 desiredPosition = player.position + offset;

            // Smoothly interpolate between the start position and the desired position
            transform.position = Vector3.Lerp(startPosition, desiredPosition, t);

            // Keep the initial rotation during the transition
            transform.rotation = initialRotation;

            // Increase the elapsed time
            elapsedTime += Time.deltaTime;

            // Wait until the next frame
            yield return null;
        }

        // Set the final position to the desired position
        transform.position = player.position + offset;

        // Ensure the camera maintains its initial rotation
        transform.rotation = initialRotation;

        // End the transition
        isTransitioning = false;
    }
}
