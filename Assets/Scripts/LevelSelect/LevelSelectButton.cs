using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelSelectButton : MonoBehaviour
{
    public string levelSceneName;
    public int UnlockAfterLevel;
    public int saveSlot = 1;
    public AudioClip levelMusic;
    public TMP_Text bestTimeText;
    public bool isTeleport = false;
    public Vector3 teleportTarget;

    void Start()
    {
        if (isTeleport) return;
        saveSlot = PlayerPrefs.GetInt("SelectedSlot", 1);
        PlayerData data = SaveSystem.LoadPlayer(saveSlot);
        int levelsCompleted = data.levelsCompleted;
        if (levelsCompleted < UnlockAfterLevel)
        {
            gameObject.SetActive(false);
        }

        if (bestTimeText != null)
        {
            float time = data.GetBestTime(levelSceneName);
            if (time > 0)
            {
                // Format: 01:23.456
                int minutes = Mathf.FloorToInt(time / 60F);
                int seconds = Mathf.FloorToInt(time % 60F);
                int milliseconds = Mathf.FloorToInt((time * 1000F) % 1000F);
                bestTimeText.text = string.Format("Best: {0:00}:{1:00}.{2:000}", minutes, seconds, milliseconds);
            }
            else
            {
                bestTimeText.text = "Best: --:--";
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isTeleport)
            {
                other.transform.position = teleportTarget;
            } else {
                MusicManager.Instance?.PlayMusic(levelMusic);
                SceneTransition.Instance.TransitionToScene(levelSceneName);
            }
        }
    }

    void OnDrawGizmos()
    {
        if (isTeleport)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, teleportTarget);
            Gizmos.DrawSphere(teleportTarget, 0.5f);
        }
    }
}
