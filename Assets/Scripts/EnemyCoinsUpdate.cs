using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCoinsUpdate : MonoBehaviour
{
    public int coinsRewarded = 10;   // Amount of coins rewarded for killing the enemy
    public CoinManager coinManager;  // Reference to the CoinManager

    // This method will be called when the enemy is shot
    public void OnEnemyShot()
    {
        // Destroy the enemy and add coins to the player
        coinManager.AddCoins(coinsRewarded);
        Destroy(gameObject);  // Destroy the enemy
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))  // Check if the enemy was hit by a bullet
        {
            OnEnemyShot();
        }
    }
}
