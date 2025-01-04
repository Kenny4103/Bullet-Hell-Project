using UnityEngine;
using System;

public class Enemy : MonoBehaviour, IEnemy
{
    public event System.Action OnEnemyDestroyed; // Event for when the enemy is destroyed

    public Transform player;
    public float moveSpeed = 2f;

    private Rigidbody2D rb;

    public Vector2 spawnAreaMin;
    public Vector2 spawnAreaMax;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    private void Update()
    {
        if (GameManager.Instance.isGameplayActive && player != null)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.velocity = direction * moveSpeed;
        }
        else
        {
            rb.velocity = Vector2.zero;
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

    // This function is called when the enemy triggers another collider
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // If the player is hit, call TakeDamage on the player's health
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1); // Inflict 1 damage
                Debug.Log("Enemy triggered with player! Player takes damage.");
            }
        }
    }
}