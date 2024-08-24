using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FractureObject : MonoBehaviour
{
    public float minForce = 10f;
    public float maxForce = 20f;
    public float radius = 5f;
    public float destroyDelay = 5f; // Time after which the fractured pieces will be destroyed

    private bool hasExploded = false;

    // This method will be called when you want the object to explode
    public void Explode()
    {
        if (hasExploded) return; // Prevent multiple explosions
        hasExploded = true;

        foreach (Transform piece in transform)
        {
            Rigidbody rb = piece.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Add explosive force to each piece
                rb.AddExplosionForce(Random.Range(minForce, maxForce), transform.position, radius);
            }

            // Destroy each piece after a delay
            Destroy(piece.gameObject, destroyDelay);
        }

        // Destroy the parent object after all pieces have exploded
        Destroy(gameObject, destroyDelay);
    }
}
