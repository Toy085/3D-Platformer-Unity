using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlotUI : MonoBehaviour
{
    public TextMeshProUGUI slotText;
    public Button slotButton;
    public TextMeshProUGUI buttonText;
    public Button deleteButton;
    private int slotIndex;
    private SaveMenu saveMenu;

    public void Setup(int index, bool hasSave, SaveMenu menu)
    {
        slotIndex = index;
        saveMenu = menu;

        if (hasSave)
            slotText.text = $"Save n. {index}";
        else
            slotText.text = $"Empty Slot n. {index}";

        slotButton.onClick.RemoveAllListeners();
        slotButton.onClick.AddListener(OnClick);
        buttonText.text = hasSave ? "Load" : "New Game";

        deleteButton.gameObject.SetActive(hasSave);
        deleteButton.onClick.RemoveAllListeners();
        deleteButton.onClick.AddListener(OnDelete);
    }

    public void OnClick()
    {
        saveMenu.OnSlotSelected(slotIndex);
    }
    public void OnDelete()
    {
        saveMenu.OnDeleteSlot(slotIndex);
    }
}
