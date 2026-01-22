using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager Instance;

    [Header("UI")]
    public GameObject popupPanel;
    public TextMeshProUGUI popupText;

    [Header("Settings")]
    public float displayDuration = 2f;

    private Coroutine currentCoroutine;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }

        popupPanel?.SetActive(false);
    }

    public void ShowMessage(string message)
    {
        popupText.text = message;

        popupPanel.SetActive(true);

        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = StartCoroutine(HidePopupRoutine());
    }

    IEnumerator HidePopupRoutine()
    {
        yield return new WaitForSecondsRealtime(displayDuration); 

        popupPanel.SetActive(false);
    }
}
