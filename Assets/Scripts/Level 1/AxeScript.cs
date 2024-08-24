using UnityEngine;

public class AxeScript : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Deal damage to the player
            PlayerScript playerScript = collision.gameObject.GetComponent<PlayerScript>();
            if (playerScript != null)
            {
                playerScript.TakeDamage(1); // Adjust damage as needed
            }
        }

        // Destroy the axe after hitting something
        Destroy(gameObject);
    }
}
