using UnityEngine;

public class LevelSelect : MonoBehaviour
{
    public AudioClip levelSelectMusic;

    public void Start()
    {
        MusicManager.Instance?.PlayMusic(levelSelectMusic);
    }
}
