using UnityEngine;

public class Enemy_Circle : MonoBehaviour, IEnemy
{
    public event System.Action OnEnemyDestroyed; // Event for when the enemy is destroyed

    public float moveSpeed = 2f; // Speed of up and down movement
    public float moveRange = 3f; // Range of movement from the starting position
    public float shootInterval = 3f; // Time between circular projectile bursts
    public GameObject projectilePrefab; // Projectile prefab to shoot
    public Transform projectileSpawner; // Spawner object for the projectiles
    public int numberOfProjectiles = 8; // Number of projectiles in the circular burst

    public Vector2 spawnAreaMin;
    public Vector2 spawnAreaMax;
    private Vector2 startPosition;
    private bool movingUp = true; // Direction of movement
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

    // Handles the up and down movement
    private void Move()
    {
        float displacement = moveSpeed * Time.deltaTime;
        if (movingUp)
        {
            transform.position += new Vector3(0, displacement, 0);

            // Reverse direction if reaching the move range
            if (transform.position.y >= startPosition.y + moveRange)
            {
                movingUp = false;
            }
        }
        else
        {
            transform.position -= new Vector3(0, displacement, 0);

            // Reverse direction if reaching the move range
            if (transform.position.y <= startPosition.y - moveRange)
            {
                movingUp = true;
            }
        }
    }

    // Handles shooting projectiles in a circular pattern
    private void ShootProjectiles()
    {
        if (projectilePrefab == null || projectileSpawner == null || !GameManager.Instance.isGameplayActive) return;

        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0f)
        {
            shootTimer = shootInterval; // Reset the timer
            ShootCircularBurst();
        }
    }

    // Spawns projectiles in a circular pattern
    private void ShootCircularBurst()
    {
        float angleStep = 360f / numberOfProjectiles; // Equal spacing for each projectile
        float angle = 0f;

        for (int i = 0; i < numberOfProjectiles; i++)
        {
            GameObject projectile = Instantiate(projectilePrefab, projectileSpawner.position, Quaternion.identity);

            // Calculate the direction for the projectile
            float projectileDirX = Mathf.Cos(angle * Mathf.Deg2Rad);
            float projectileDirY = Mathf.Sin(angle * Mathf.Deg2Rad);
            Vector2 projectileDirection = new Vector2(projectileDirX, projectileDirY).normalized;

            // Assign the direction to the projectile
            Projectile projectileScript = projectile.GetComponent<Projectile>();
            if (projectileScript != null)
            {
                projectileScript.SetDirection(projectileDirection);
            }

            angle += angleStep;
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
        float clampedX = Mathf.Clamp(transform.position.x, spawnAreaMin.x, spawnAreaMax.x);
        float clampedY = Mathf.Clamp(transform.position.y, spawnAreaMin.y, spawnAreaMax.y);

        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }
}
