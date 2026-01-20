using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectButton : MonoBehaviour
{
    public string levelSceneName;
    public int UnlockAfterLevel;
    public int saveSlot = 1;
    public AudioClip levelMusic;

    void Start()
    {
        saveSlot = PlayerPrefs.GetInt("SelectedSlot", 1);
        PlayerData data = SaveSystem.LoadPlayer(saveSlot);
        int levelsCompleted = data.levelsCompleted;
        if (levelsCompleted < UnlockAfterLevel)
        {
            gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            MusicManager.Instance?.PlayMusic(levelMusic);
            SceneTransition.Instance.TransitionToScene(levelSceneName);
        }
    }
}
