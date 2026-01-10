using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("LevelSelect"); // Change to save menu later
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
