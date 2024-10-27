using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    public Button[] levelButtons;
    public Button exitButton; // Reference for the Exit button

    void Start()
    {
        // Set up level buttons to load the corresponding levels
        for (int i = 0; i < levelButtons.Length; i++)
        {
            int levelIndex = i + 1;
            levelButtons[i].onClick.AddListener(() => LoadLevel(levelIndex));
        }

        // Add the listener for the exit button
        exitButton.onClick.AddListener(ExitGame);
    }

    void LoadLevel(int levelIndex)
    {
        SceneManager.LoadScene("Level" + levelIndex);
    }

    void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Only for testing in the editor
#else
            Application.Quit(); // Quit the application in a build
#endif
    }
}
