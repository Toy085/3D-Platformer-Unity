using UnityEngine;
using UnityEngine.UI;
using System;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;
    public GameObject creditsPanel;
    public GameObject saveMenuPanel;
    public AudioClip Music;

    private bool isCredits = false;

    void Start()
    {
        AudioListener.volume = PlayerPrefs.GetFloat("Volume", 1f);

        bool isFullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
        Screen.fullScreen = isFullscreen;

        MusicManager.Instance?.PlayMusic(Music);
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

    public void Credits()
    {
        if (!isCredits)
        {
            creditsPanel.SetActive(true);
            isCredits = true;
        } else
        {
            creditsPanel.SetActive(false);
            isCredits = false;
        }
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}
