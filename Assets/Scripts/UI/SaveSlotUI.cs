using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlotUI : MonoBehaviour
{
    public TextMeshProUGUI slotText;
    public Button slotButton;
    public TextMeshProUGUI buttonText;
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

        slotButton.onClick.AddListener(OnClick);
        buttonText.text = hasSave ? "Load" : "New Game";
    }

    public void OnClick()
    {
        saveMenu.OnSlotSelected(slotIndex);
    }
}
