using UnityEngine;
using UnityEngine.UI;
using System;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;
    public GameObject saveMenuPanel;

    void Start()
    {
        AudioListener.volume = PlayerPrefs.GetFloat("Volume", 1f);

        bool isFullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
        Screen.fullScreen = isFullscreen;
    }

    public void OpenSettings()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }
    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
    public void OpenSaveMenu()
    {
        mainMenuPanel.SetActive(false);
        saveMenuPanel.SetActive(true);
    }
    public void CloseSaveMenu()
    {
        saveMenuPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
