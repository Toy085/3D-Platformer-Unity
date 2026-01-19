using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public GameObject settingsPanel;
    public Toggle fullscreenToggle;
    public Slider volumeSlider;
    public Slider musicVolumeSlider;
    public Slider sensitivitySlider;

    void Start()
    {
        AudioListener.volume = PlayerPrefs.GetFloat("Volume", 1f);
        volumeSlider.value = AudioListener.volume;

        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        musicVolumeSlider.value = musicVolume;
        MusicManager.Instance?.SetMusicVolume(musicVolume);

        bool isFullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
        Screen.fullScreen = isFullscreen;
        if (fullscreenToggle != null)
        {
            fullscreenToggle.isOn = isFullscreen;
        }

        if (sensitivitySlider != null)
        {
            sensitivitySlider.value = PlayerPrefs.GetFloat("Sensitivity", 1f);
        }
    }
    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }
    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }

    public void SetVolume(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("Volume", value);
    }
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
    }
    public void SetSensitivity(float value)
    {
        PlayerPrefs.SetFloat("Sensitivity", value);
    }
    public void SetMusicVolume(float value)
    {
        MusicManager.Instance?.SetMusicVolume(value);
        PlayerPrefs.SetFloat("MusicVolume", value);
    }
}
