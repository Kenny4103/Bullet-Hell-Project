using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadLevelSelect()
    {
        SceneManager.LoadScene("LevelSelect");
    }
}