using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class HUDUI : MonoBehaviour
{
    public TextMeshProUGUI coinText;

    public GameObject pausePanel;
    private bool isPaused = false;
    public PlayerInput playerInput;
    public GameObject fade;

    public void SetCoinUI(int amount)
    {
        coinText.text = "Coins: " + amount.ToString();
    }

    public void OnPause(InputValue value)
    {
        if (!value.isPressed) return;

        if (isPaused)
            Resume();
        else
            Pause();
    }

    public void Pause()
    {
        playerInput.SwitchCurrentActionMap("UI");
        pausePanel.SetActive(true);
        fade.SetActive(false);
        Time.timeScale = 0f;
        isPaused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Resume()
    {
        playerInput.SwitchCurrentActionMap("Player");
        pausePanel.SetActive(false);
        fade.SetActive(true);
        Time.timeScale = 1f;
        isPaused = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }
}
