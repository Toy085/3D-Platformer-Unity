using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectButton : MonoBehaviour
{
    public string levelSceneName;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Loading level: " + levelSceneName);
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(levelSceneName);
        }
    }
}
