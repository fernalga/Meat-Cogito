using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    public GameObject settingsMenu;
    public PauseMenu pauseMenu;
    private bool isSettingsOpen = false;

    public AudioMixer audioMixers;
    public TMPro.TMP_Dropdown resolutionDropdown;
    public TMPro.TMP_Dropdown qualityDropdown;
    public Slider volSlider;
    public Toggle fullScreenToggle;
    
    public PlayerCamera playerCamera;
    public Slider mouseSensitivitySlider;
    
    public CanvasGroup mainMenuCanvasGroup;
    
    Resolution[] resolutions;

    private string settingsFilePath;

    void Start()
    {
        // Set the path where the settings file will be saved (use persistent data path)
        settingsFilePath = Path.Combine(Application.persistentDataPath, "settings.json");

        // Deactivate pause menu on start
        if (settingsMenu != null)
            settingsMenu.SetActive(false);

        volSlider.onValueChanged.AddListener(SetVolume);

        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        
        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height + " @ " + resolutions[i].refreshRate +
                            "hz";
            options.Add(option);
            
            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height &&
                resolutions[i].refreshRate == Screen.currentResolution.refreshRate)
            {
                currentResolutionIndex = i;

            }
        }
        
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
        
        // Load settings from JSON file if it exists
        LoadSettings();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isSettingsOpen)
        {
            CloseSettings();
        }
    }

    public void SetVolume(float volume)
    {
        audioMixers.SetFloat("Volume", Mathf.Log10(volume) * 20);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        qualityDropdown.value = qualityIndex;
        qualityDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
        fullScreenToggle.isOn = isFullScreen;
    }

    public void SetMouseSensitivity(float sensitivity)
    {
        playerCamera.mouseSensitivity = sensitivity;
    }

    public void OpenSettings()
    {
        isSettingsOpen = true;
        settingsMenu.SetActive(true);

        // Disable interaction with the main menu UI (background) while settings are open
        if (mainMenuCanvasGroup != null)
        {
            mainMenuCanvasGroup.interactable = false;
            mainMenuCanvasGroup.blocksRaycasts = false;
        }

        // Disable pause menu interaction while settings are open
        if (pauseMenu != null)
            pauseMenu.PauseMenuUI.SetActive(false);
    }

    public void CloseSettings()
    {
        isSettingsOpen = false;
        settingsMenu.SetActive(false);

        // Re-enable the pause menu UI if the game is still paused
        if (pauseMenu != null && PauseMenu.isPaused)
            pauseMenu.PauseMenuUI.SetActive(true);

        if (mainMenuCanvasGroup != null)
        {
            mainMenuCanvasGroup.interactable = true;
            mainMenuCanvasGroup.blocksRaycasts = true;
        }

        // Save the settings to the file when closing the settings menu
        SaveSettings();
    }

    public bool IsSettingsOpen()
    {
        return isSettingsOpen;
    }

    // Save settings to JSON file
    private void SaveSettings()
    {
        SettingsData settings = new SettingsData
        {
            volume = volSlider.value,
            quality = QualitySettings.GetQualityLevel(),
            resolutionIndex = resolutionDropdown.value,
            isFullScreen = Screen.fullScreen,
            mouseSensitivity = mouseSensitivitySlider.value
        };

        string json = JsonUtility.ToJson(settings);
        File.WriteAllText(settingsFilePath, json); // Save JSON to file
        
        Debug.Log(Application.persistentDataPath);
    }

    // Load settings from JSON file
    private void LoadSettings()
    {
        if (File.Exists(settingsFilePath))
        {
            string json = File.ReadAllText(settingsFilePath);
            SettingsData settings = JsonUtility.FromJson<SettingsData>(json);

            volSlider.value = settings.volume;
            SetVolume(settings.volume);
            
            mouseSensitivitySlider.value = settings.mouseSensitivity;
            SetMouseSensitivity(settings.mouseSensitivity);
            
            resolutionDropdown.value = settings.resolutionIndex;
            fullScreenToggle.isOn = settings.isFullScreen;
            SetResolution(settings.resolutionIndex);
            
            SetFullScreen(settings.isFullScreen);
            
            qualityDropdown.value = settings.quality;
            SetQuality(settings.quality);
        }
    }
}
