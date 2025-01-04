using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 5f; // Speed of the projectile
    private Vector2 moveDirection;

    private void Update()
    {
        // Move the projectile
        transform.position += (Vector3)moveDirection * speed * Time.deltaTime;
    }

    // Set the direction for the projectile
    public void SetDirection(Vector2 direction)
    {
        moveDirection = direction.normalized;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Damage the player
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1);
            }

            // Destroy the projectile
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            // Destroy the projectile on wall collision
            Destroy(gameObject);
        }
    }
}

