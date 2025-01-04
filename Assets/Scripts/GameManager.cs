using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Text levelText; // Assign this in the Inspector
    public Text controlsText; // Assign this in the Inspector for the controls
    public float gameOverTextDuration = 3f; // Duration to display "Game Over" text
    public float controlsTextDuration = 5f; // Duration to display the controls text
    public event Action OnGameplayStart; // Event for when gameplay starts
    public bool isGameplayActive { get; private set; } = false;

    private int currentLevel = 1; // Track the current level
    private int totalLevels = 3; // Total number of levels (adjust as necessary)
    public float levelTransitionDelay = 2f; // Delay before transitioning to the next level
    public float winSceneDelay = 1f; // Delay before showing the WinScene

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartGameplay()
    {
        isGameplayActive = true;

        // Trigger the event
        OnGameplayStart?.Invoke();

        Debug.Log($"Gameplay started on Level {currentLevel}!");

        // Display the level start text
        DisplayStartText();
    }

    private void DisplayStartText()
    {
        levelText.text = $"Start!";
        levelText.gameObject.SetActive(true);

        // Schedule the controls text to appear after the start text disappears
        StartCoroutine(DisplayControlsText());
    }

    private IEnumerator DisplayControlsText()
    {
        yield return new WaitForSeconds(2f);

        levelText.gameObject.SetActive(false);

        if (controlsText != null)
        {
            controlsText.text = "Move with WASD, Shoot with K";
            controlsText.gameObject.SetActive(true);

            yield return new WaitForSeconds(controlsTextDuration);
            controlsText.gameObject.SetActive(false);
        }
    }

    public void OnAllEnemiesDefeated()
    {
        StartCoroutine(LevelTransition());
    }

    private IEnumerator LevelTransition()
    {
        levelText.text = $"Level Complete!";
        levelText.gameObject.SetActive(true);

        // Wait for a short delay before transitioning to the WinScene
        yield return new WaitForSeconds(winSceneDelay);

        SceneManager.LoadScene("WinScene");

        yield return new WaitForSeconds(levelTransitionDelay);

        if (currentLevel < totalLevels)
        {
            currentLevel++;
            string nextSceneName = $"Level{currentLevel}";

            // Debugging log to ensure the scene name is correct
            Debug.Log($"Attempting to load next scene: {nextSceneName}");

            if (SceneExists(nextSceneName))
            {
                SceneManager.LoadScene(nextSceneName);
            }
            else
            {
                Debug.LogError($"Scene '{nextSceneName}' does not exist in Build Settings!");
            }
        }
        else if (currentLevel == totalLevels)
        {
            Debug.Log("All levels completed! Transitioning to Victory scene.");

            if (SceneExists("WinScene"))
            {
                SceneManager.LoadScene("WinScene");
            }
            else
            {
                Debug.LogError("WinScene does not exist in Build Settings!");
            }
        }
    }

    // Helper function to verify if a scene exists in Build Settings
    private bool SceneExists(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            string name = System.IO.Path.GetFileNameWithoutExtension(path);

            if (name == sceneName)
            {
                return true;
            }
        }
        return false;
    }

    public void ScheduleGameOverText(float delay)
    {
        StartCoroutine(DisplayGameOverTextAfterDelay(delay));
    }

    private IEnumerator DisplayGameOverTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ShowGameOverText();
    }

    private void ShowGameOverText()
    {
        levelText.text = "Game Over";
        levelText.gameObject.SetActive(true);

        StartCoroutine(ReturnToLevelSelectAfterDelay());
    }

    private IEnumerator ReturnToLevelSelectAfterDelay()
    {
        yield return new WaitForSeconds(gameOverTextDuration);

        levelText.gameObject.SetActive(false);
        SceneManager.LoadScene("LevelSelect");
    }
}
