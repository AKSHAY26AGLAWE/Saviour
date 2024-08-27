using UnityEngine;
using TMPro;  

public class CoinManager : MonoBehaviour
{
    public int coins = 0; 
    public TextMeshProUGUI coinText; 

    // Method to add coins
    public void AddCoins(int amount)
    {
        coins += amount;
        UpdateCoinDisplay(); 
        Debug.Log("Coins added: " + amount + ". Total Coins: " + coins);
    }

    // Method to update the coin display
    void UpdateCoinDisplay()
    {
        if (coinText != null)
        {
            coinText.text = coins.ToString();
        }
    }
}
