using UnityEngine;
using System;

public class Enemy_Shooter : MonoBehaviour, IEnemy
{
    public event System.Action OnEnemyDestroyed; // Event for when the enemy is destroyed
    public float moveSpeed = 2f; // Speed of the left and right movement
    public float moveRange = 5f; // Range of movement from the starting position
    public float shootInterval = 3f; // Time between projectile bursts
    public GameObject projectilePrefab; // Projectile prefab to shoot
    public Transform projectileSpawner; // Spawner object for the projectiles

    public Vector2 spawnAreaMin;
    public Vector2 spawnAreaMax;
    public Vector2 startPosition;
    private bool movingRight = true; // Direction of movement
    private Transform player; // Reference to the player's transform
    private float shootTimer;

    private void Start()
    {
        startPosition = transform.position;

        // Find the player by tag
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        // Initialize the shoot timer
        shootTimer = shootInterval;
    }

    private void Update()
    {
        Move();
        ShootProjectiles();
    }

    // Handles the left and right movement
    private void Move()
    {
        float displacement = moveSpeed * Time.deltaTime;
        if (movingRight)
        {
            transform.position += new Vector3(displacement, 0, 0);

            // Reverse direction if reaching the move range
            if (transform.position.x >= startPosition.x + moveRange)
            {
                movingRight = false;
            }
        }
        else
        {
            transform.position -= new Vector3(displacement, 0, 0);

            // Reverse direction if reaching the move range
            if (transform.position.x <= startPosition.x - moveRange)
            {
                movingRight = true;
            }
        }
    }

    // Handles shooting projectiles at regular intervals
    private void ShootProjectiles()
    {
        if (player == null || !GameManager.Instance.isGameplayActive) return;

        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0f)
        {
            shootTimer = shootInterval; // Reset the timer
            ShootBurst();
        }
    }

    // Spawns three projectiles targeting the player
    private void ShootBurst()
    {
        if (projectilePrefab == null || projectileSpawner == null) return;

        for (int i = -1; i <= 1; i++) // Loop to create three projectiles
        {
            GameObject projectile = Instantiate(projectilePrefab, projectileSpawner.position, Quaternion.identity);

            // Calculate a slightly adjusted direction for each projectile
            Vector2 directionToPlayer = (player.position - projectileSpawner.position).normalized;
            float angleAdjustment = i * 15f; // Adjust angle by -15, 0, and +15 degrees
            Vector2 adjustedDirection = Quaternion.Euler(0, 0, angleAdjustment) * directionToPlayer;

            // Assign the direction to the projectile
            Projectile projectileScript = projectile.GetComponent<Projectile>();
            if (projectileScript != null)
            {
                projectileScript.SetDirection(adjustedDirection);
            }
        }
    }
    private void OnDestroy()
    {
        // Trigger the event when the enemy is destroyed
        OnEnemyDestroyed?.Invoke();
    }
    public void ConfigureSpawnArea(Vector2 min, Vector2 max)
    {
        spawnAreaMin = min;
        spawnAreaMax = max;
    }

    // This function clamps the enemy's position to ensure it stays within the screen bounds
    private void ClampPosition()
    {
        // Clamp the position of the enemy within the screen bounds
        float clampedX = Mathf.Clamp(transform.position.x, spawnAreaMin.x, spawnAreaMax.x);
        float clampedY = Mathf.Clamp(transform.position.y, spawnAreaMin.y, spawnAreaMax.y);

        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }

}
