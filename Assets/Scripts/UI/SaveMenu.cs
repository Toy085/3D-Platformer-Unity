using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SaveMenu : MonoBehaviour
{
    public Transform content;
    public SaveSlotUI slotPrefab;

    public int maxSlots = 10;

    private void Start()
    {
        PopulateSlots();
        LayoutRebuilder.ForceRebuildLayoutImmediate(
            content.GetComponent<RectTransform>()
        );
    }

    void PopulateSlots()
    {
        foreach (Transform child in content)
            Destroy(child.gameObject);

        for (int i = 1; i <= maxSlots; i++)
        {
            bool hasSave = SaveSystem.SaveExists(i);

            SaveSlotUI slot = Instantiate(slotPrefab, content);
            slot.Setup(i, hasSave, this);
        }
    }

    public void OnSlotSelected(int slot)
    {
        if (SaveSystem.SaveExists(slot))
        {
            PlayerPrefs.SetInt("SelectedSlot", slot);
            SceneManager.LoadScene("LevelSelect");
        }
        else
        {
            PlayerPrefs.SetInt("SelectedSlot", slot);
            SaveSystem.DeleteSave(slot);
            SceneManager.LoadScene("LevelSelect");
        }
    }

    public void OnDeleteSlot(int slot)
    {
        SaveSystem.DeleteSave(slot);
        PopulateSlots();
    }
}
