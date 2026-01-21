using UnityEngine;
using System.Collections;

public class SpeedrunTimer : MonoBehaviour
{
    public HUDUI hudUI;
    public bool isRunning = false;
    private float elapsedTime = 0f;

    private void Start()
    {
        isRunning = true;
        elapsedTime = 0f;
    }

    private void Update()
    {
        if (isRunning)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimerUI();
        }
    }

    public void StopTimer()
    {
        isRunning = false;
        UpdateTimerUI();
    }

    private void UpdateTimerUI()
    {
        if (hudUI != null && hudUI.timerText != null)
        {
            int minutes = Mathf.FloorToInt(elapsedTime / 60F);
            int seconds = Mathf.FloorToInt(elapsedTime % 60F);
            int milliseconds = Mathf.FloorToInt((elapsedTime * 1000F) % 1000F);

            hudUI.timerText.text = string.Format("{0:00}:{1:00}.{2:000}", minutes, seconds, milliseconds);
        }
    }
    
    public float GetFinalTime()
    {
        return elapsedTime;
    }
}