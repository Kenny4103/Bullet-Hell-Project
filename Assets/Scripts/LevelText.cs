using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelStartText : MonoBehaviour
{
    public Text levelText;
    public float levelDisplayTime = 2f;
    public float startDisplayTime = 1f;
    public int levelNumber = 1;

    private IEnumerator DisplayLevelStartText()
    {
        levelText.text = $"Level {levelNumber}";
        levelText.gameObject.SetActive(true);
        yield return new WaitForSeconds(levelDisplayTime);

        levelText.text = "Start!";
        yield return new WaitForSeconds(startDisplayTime);

        levelText.gameObject.SetActive(false);

        // Enable gameplay
        GameManager.Instance.StartGameplay();
    }

    private void Start()
    {
        StartCoroutine(DisplayLevelStartText());
    }
}
