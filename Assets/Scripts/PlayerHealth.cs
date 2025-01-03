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
            // Trigger Game Over logic
            GameOver();
        }
    }

    // Handle Game Over logic
    private void GameOver()
    {
        // Log Game Over to the console
        Debug.Log("Game Over! Player is dead.");

        // Stop player movement or other gameplay elements
        PlayerMovement playerMovement = GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }

        // Ensure the player remains stationary
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.isKinematic = true; // Optional: Disable physics
        }

        // Notify the GameManager to show the Game Over text after the player is destroyed
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ScheduleGameOverText(1f); // 1-second delay to match object destruction
        }

        // Destroy the player GameObject after a short delay
        Destroy(gameObject, 1f); // Adjust the delay as needed (e.g., 1 second)
    }
}