using TMPro;
using UnityEngine;

public class HUDUI : MonoBehaviour
{
    public TextMeshProUGUI coinText;

    public void setCoinUI(int amount)
    {
        coinText.text = "Coins: " + amount.ToString();
    }
}
