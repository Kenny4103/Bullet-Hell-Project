using UnityEngine;

public class ProjectileSpawn : MonoBehaviour
{
    [Header("Projectile Settings")]
    [Tooltip("The prefab of the projectile to spawn.")]
    public GameObject projectilePrefab;

    [Tooltip("The speed at which the projectile moves upwards.")]
    public float projectileSpeed = 5f;

    [Tooltip("The lifetime of the projectile before despawning (in seconds).")]
    public float projectileLifetime = 5f;

    [Tooltip("The GameObject that determines the spawn position of the projectile.")]
    public Transform projectileSpawner;

    void Update()
    {
        // Spawn a projectile when the spacebar is pressed
        if (Input.GetKeyDown(KeyCode.K))
        {
            SpawnProjectile();
        }
    }

    void SpawnProjectile()
    {
        if (projectilePrefab == null)
        {
            Debug.LogError("Projectile prefab is not assigned!");
            return;
        }

        if (projectileSpawner == null)
        {
            Debug.LogError("ProjectileSpawner Transform is not assigned!");
            return;
        }

        // Instantiate the projectile at the position of the ProjectileSpawner GameObject
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawner.position, Quaternion.identity);

        // Configure the projectile's movement and despawn behavior
        ProjectileMover mover = projectile.AddComponent<ProjectileMover>();
        mover.speed = projectileSpeed;
        mover.lifetime = projectileLifetime;
    }
}

public class ProjectileMover : MonoBehaviour
{
    public float speed;    // Movement speed
    public float lifetime; // Lifetime before despawning

    private float timer; // Tracks how long the projectile has existed

    void Update()
    {
        // Move the projectile upwards in world space
        transform.Translate(Vector3.up * speed * Time.deltaTime, Space.World);

        // Increment timer and despawn after lifetime expires
        timer += Time.deltaTime;
        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Projectile collided with: {other.gameObject.name}");

        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Enemy hit!");
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                Debug.Log("EnemyHealth component found. Applying damage.");
                enemyHealth.TakeDamage(1); // Inflict damage
            }
            else
            {
                Debug.LogError("EnemyHealth component missing!");
            }

            Destroy(gameObject);
        }
    }


}

