using UnityEngine;
using System.Collections.Generic;

public class BikePathFollower : MonoBehaviour
{
    public List<Transform> waypoints;
    public float speed = 5f;
    public float rotationSpeed = 5f;

    private int currentWaypointIndex = 0;
    public Vector3 manualRotationOffset = new Vector3(0, 0, 0); // Manually set this to match the bike's correct orientation

    void Start()
    {
        if (waypoints.Count == 0)
        {
            Debug.LogError("No waypoints set for the bike to follow.");
            return;
        }

        // Position the bike at the first waypoint
        transform.position = waypoints[currentWaypointIndex].position;

        // Apply the manual rotation offset to the bike's rotation
        transform.rotation = Quaternion.Euler(manualRotationOffset);
    }

    void Update()
    {
        if (currentWaypointIndex < waypoints.Count)
        {
            MoveTowardsWaypoint();
        }
    }

    void MoveTowardsWaypoint()
    {
        Transform targetWaypoint = waypoints[currentWaypointIndex];

        // Calculate direction to the next waypoint
        Vector3 direction = (targetWaypoint.position - transform.position).normalized;

        // Move towards the waypoint
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, speed * Time.deltaTime);

        // Calculate the target rotation based on the direction towards the waypoint and apply the manual rotation offset
        Quaternion targetRotation = Quaternion.LookRotation(direction) * Quaternion.Euler(manualRotationOffset);

        // Smoothly rotate towards the target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Check if the bike has reached the waypoint
        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            currentWaypointIndex++;
        }
    }
}
