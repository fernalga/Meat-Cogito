using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadLevel1()
    {
        LoadLevel("Level1");
    }

    public void LoadNextInBuild()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            LoadLevel(nextSceneIndex);
        }
        else
        {
            Debug.LogWarning("No next scene in build settings.");
        }
    }

    public void LoadSceneByName(string sceneName)
    {
        LoadLevel(sceneName);
    }
    
    private void LoadLevel(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);
    }
    
    private void LoadLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
