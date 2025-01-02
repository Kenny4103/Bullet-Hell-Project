using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public float moveSpeed = 2f; // Speed of movement towards the player

    private Rigidbody2D rb;

    // The screen bounds for random position (optional)
    private float minX, maxX, minY, maxY;

    // Range for random spawn within a specific area
    public Vector2 spawnAreaMin;  // Minimum point for random spawn (X, Y)
    public Vector2 spawnAreaMax;  // Maximum point for random spawn (X, Y)

    void Start()
    {
        // Get the Rigidbody2D component to control physics-based movement
        rb = GetComponent<Rigidbody2D>();

        // Randomize enemy spawn position within the defined range
        RandomizePosition();

        // Optional: Set screen bounds based on the camera for clamping (if desired)
        Camera mainCamera = Camera.main;
        float cameraHeight = mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        // Set bounds based on camera's position and screen size
        minX = -cameraWidth;
        maxX = cameraWidth;
        minY = -cameraHeight;
        maxY = cameraHeight;
    }

    void Update()
    {
        if (player != null)
        {
            // Calculate direction towards the player
            Vector2 direction = (player.position - transform.position).normalized;

            // Move the enemy smoothly towards the player using Rigidbody2D
            rb.velocity = direction * moveSpeed;
        }

        // Clamp the enemy's position to keep it on the screen
        ClampPosition();
    }

    // Randomize the enemy's position within the defined range
    private void RandomizePosition()
    {
        float randomX = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
        float randomY = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
        transform.position = new Vector3(randomX, randomY, transform.position.z);

        Debug.Log("Enemy spawned at: " + transform.position);
    }

    // This function clamps the enemy's position to ensure it stays within the screen bounds
    private void ClampPosition()
    {
        // Clamp the position of the enemy within the screen bounds
        float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
        float clampedY = Mathf.Clamp(transform.position.y, minY, maxY);

        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }

    // This function is called when the enemy collides with another collider
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // If the player is hit, call TakeDamage on the player's health
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1); // Inflict 1 damage
                Debug.Log("Enemy collided with player! Player takes damage.");
            }
        }
    }
}
