using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Text levelText; // Assign this in the Inspector
    public float gameOverTextDuration = 3f; // Duration to display "Game Over" text
    public bool isGameplayActive = false;

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
        isGameplayActive = true; // Enable gameplay
    }

    // Schedule the Game Over text to display after a delay
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

        // Start the coroutine to return to the LevelSelect scene after the text duration
        StartCoroutine(ReturnToLevelSelectAfterDelay());
    }

    private IEnumerator ReturnToLevelSelectAfterDelay()
    {
        yield return new WaitForSeconds(gameOverTextDuration);

        // Hide the text and load the LevelSelect scene
        levelText.gameObject.SetActive(false);
        SceneManager.LoadScene("LevelSelect");
    }
}
