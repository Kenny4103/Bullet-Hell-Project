using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawn : MonoBehaviour
{
    public GameObject[] enemyPrefabs; // Array of enemy prefabs (e.g., Enemy, Enemy_Shooter, etc.)
    public Vector2 spawnAreaMin; // Minimum spawn area (X, Y)
    public Vector2 spawnAreaMax; // Maximum spawn area (X, Y)

    public Text enemiesRemainingText; // Reference to the UI Text object
    private int enemiesRemaining; // Counter for remaining enemies
    private int currentWave = 0; // Tracks the current wave
    public int totalWaves = 3; // Total number of waves
    public float waveDelay = 3f; // Delay before the next wave starts

    private void Start()
    {
        // Subscribe to the GameManager event when gameplay starts
        GameManager.Instance.OnGameplayStart += StartWaves;

        // Ensure the text is initialized
        UpdateEnemiesRemainingText();
    }

    private void OnDestroy()
    {
        // Unsubscribe from the event to prevent memory leaks
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameplayStart -= StartWaves;
        }
    }

    private void StartWaves()
    {
        currentWave = 1; // Start from the first wave
        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        if (currentWave > totalWaves)
        {
            Debug.Log("All waves completed!");
            GameManager.Instance.OnAllEnemiesDefeated(); // Notify GameManager to transition levels
            yield break; // End wave spawning
        }

        Debug.Log($"Starting wave {currentWave}");

        // Determine the number of enemies for this wave
        int enemiesToSpawn = Random.Range(4, 8); // Random number between 4 and 7 (inclusive)
        enemiesRemaining = enemiesToSpawn; // Initialize the counter
        UpdateEnemiesRemainingText(); // Update the UI at the start of the wave

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            // Generate a random position within the spawn area
            Vector2 randomPosition = new Vector2(
                Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                Random.Range(spawnAreaMin.y, spawnAreaMax.y)
            );

            // Randomly select an enemy prefab
            GameObject selectedEnemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

            // Instantiate the enemy prefab at the random position
            GameObject enemy = Instantiate(selectedEnemyPrefab, randomPosition, Quaternion.identity);

            // Ensure the spawned enemy implements the IEnemy interface
            IEnemy enemyScript = enemy.GetComponent<IEnemy>();
            if (enemyScript != null)
            {
                enemyScript.ConfigureSpawnArea(spawnAreaMin, spawnAreaMax);

                // Subscribe to the enemy's destruction event
                enemyScript.OnEnemyDestroyed += HandleEnemyDestroyed;
            }
            else
            {
                Debug.LogWarning($"Spawned prefab {enemy.name} does not implement IEnemy interface!");
            }
        }

        Debug.Log($"Wave {currentWave}: {enemiesToSpawn} enemies spawned.");
    }

    private void HandleEnemyDestroyed()
    {
        enemiesRemaining--; // Decrement the counter
        UpdateEnemiesRemainingText(); // Update the UI

        // Check if all enemies in the current wave are defeated
        if (enemiesRemaining <= 0)
        {
            Debug.Log($"Wave {currentWave} completed!");
            currentWave++; // Move to the next wave

            if (currentWave <= totalWaves)
            {
                StartCoroutine(StartNextWaveAfterDelay());
            }
            else
            {
                Debug.Log("All waves completed!");
                GameManager.Instance.OnAllEnemiesDefeated(); // Notify GameManager to move to the next level
            }
        }
    }

    private IEnumerator StartNextWaveAfterDelay()
    {
        yield return new WaitForSeconds(waveDelay);
        StartCoroutine(SpawnWave());
    }

    private void UpdateEnemiesRemainingText()
    {
        if (enemiesRemainingText != null)
        {
            enemiesRemainingText.text = $"Enemies Remaining: {enemiesRemaining}";
        }
    }
}
