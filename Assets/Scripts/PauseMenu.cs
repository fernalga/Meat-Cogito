using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject PauseMenuUI;
    public SettingsMenu settingsMenu;

    public static bool isPaused;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Reset pause state on scene load
        isPaused = false;
        Time.timeScale = 1f;

        // Only hide cursor in gameplay scenes (not menus)
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            CursorManager.Instance.ReleaseCursor("PauseMenu");
        }
        else
        {
            CursorManager.Instance.RequestCursor("PauseMenu");
        }

        // Deactivate pause menu on start
        if (PauseMenuUI != null)
            PauseMenuUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // If the settings menu is open, close it first
            if (settingsMenu != null && settingsMenu.IsSettingsOpen())
            {
                settingsMenu.CloseSettings();
            }
            else
            {
                // Otherwise, toggle pause menu as usual
                if (isPaused)
                {
                    ResumeGame();
                    CursorManager.Instance.ReleaseCursor("PauseMenu");
                }
                else
                {
                    PauseGame();
                    CursorManager.Instance.RequestCursor("PauseMenu");
                }
            }
        }
    }

    public void PauseGame()
    {
        if (PauseMenuUI != null)
            PauseMenuUI.SetActive(true);
        
        Time.timeScale = 0f;
        CursorManager.Instance.RequestCursor("PauseMenu");
        isPaused = true;
    }
    
    public void ResumeGame()
    {
        if (PauseMenuUI != null)
            PauseMenuUI.SetActive(false);

        Time.timeScale = 1f;
        CursorManager.Instance.ReleaseCursor("PauseMenu");
        isPaused = false;
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        isPaused = false;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
