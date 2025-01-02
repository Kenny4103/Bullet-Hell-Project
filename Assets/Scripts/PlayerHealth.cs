using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int health = 3; // Starting health of the player

    // Method to reduce health when the player takes damage
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            // Log Game Over to the Console
            GameOver();
        }
    }

    // Handle Game Over logic
    private void GameOver()
    {
        // Log Game Over to the console
        Debug.Log("Game Over! Player is dead.");

        // Optionally, you could also stop the player movement or any other gameplay elements here
        // For example, you can disable movement:
        PlayerMovement playerMovement = GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }
    }
}
