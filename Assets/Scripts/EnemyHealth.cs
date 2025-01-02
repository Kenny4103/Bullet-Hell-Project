using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int health = 3; // Starting health of the enemy

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
            Debug.Log("Enemy destroyed!");
        }
    }
}
