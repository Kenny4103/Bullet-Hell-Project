using UnityEngine;

public interface IEnemy
{
    event System.Action OnEnemyDestroyed; // Event triggered when the enemy is destroyed
    void ConfigureSpawnArea(Vector2 min, Vector2 max); // Configures the spawn area
}
