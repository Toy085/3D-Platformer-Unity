using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;


public class Settings : MonoBehaviour
{
    public GameObject settingsPanel;
    public Toggle fullscreenToggle;
    public Slider volumeSlider;
    public Slider musicVolumeSlider;
    public Slider sensitivitySlider;
    public TMP_Dropdown resolutionDropdown;
    Resolution[] resolutions;

    void Start()
    {
        PopulateResolutions();

        int savedIndex = PlayerPrefs.GetInt("ResolutionIndex", GetCurrentResolutionIndex());
        SetResolution(savedIndex);
        resolutionDropdown.value = savedIndex;
        resolutionDropdown.RefreshShownValue();

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

    void PopulateResolutions()
    {
        Resolution[] allResolutions = Screen.resolutions;
        List<Resolution> filtered = new List<Resolution>();
        List<string> options = new List<string>();

        foreach (Resolution res in allResolutions)
        {
            if (!filtered.Exists(r => r.width == res.width && r.height == res.height))
            {
                filtered.Add(res);
                string optionRes = res.width + " x " + res.height + " @ " + Mathf.Round((float)res.refreshRateRatio.value) + "Hz";
                options.Add(optionRes);
            }
        }

        resolutions = filtered.ToArray();

        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(options);
    }
    public void SetResolution(int index)
    {
        Resolution res = resolutions[index];

        FullScreenMode mode = Screen.fullScreen
            ? FullScreenMode.FullScreenWindow
            : FullScreenMode.Windowed;

        Screen.SetResolution(res.width, res.height, mode, res.refreshRateRatio);
        PlayerPrefs.SetInt("ResolutionIndex", index);
    }

    int GetCurrentResolutionIndex()
    {
        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].width == Screen.width &&
                resolutions[i].height == Screen.height)
            {
                return i;
            }
        }
        return 0;
    }
}