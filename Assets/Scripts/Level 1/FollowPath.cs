using UnityEngine;

public class SmoothMovement : MonoBehaviour
{
    public Transform[] waypoints; // Array of waypoints
    public float speed = 5f; // Speed of the object
    private int currentWaypointIndex = 0;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    void FixedUpdate()
    {
        if (currentWaypointIndex < waypoints.Length)
        {
            MoveTowardsWaypoint();
        }
    }

    void MoveTowardsWaypoint()
    {
        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 direction = (targetWaypoint.position - rb.position).normalized;

        // Move towards the current waypoint smoothly using Rigidbody
        Vector3 movement = direction * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);

        // Check if the object has reached the waypoint
        if (Vector3.Distance(rb.position, targetWaypoint.position) < 0.1f)
        {
            currentWaypointIndex++;
        }
    }
}
