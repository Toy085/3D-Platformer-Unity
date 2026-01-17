using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemUI : MonoBehaviour
{
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemPriceText;
    public Image itemIcon;
    public Button buyButton;
    public TextMeshProUGUI buyButtonText;

    private int _itemID;
    private Shop _shopReference;

    public void Setup(ShopItem item, Shop shopScript, bool isOwned)
    {
        if (item == null) return;

        itemNameText.text = item.itemName;
        itemPriceText.text = item.price.ToString();

        _itemID = item.id;
        _shopReference = shopScript;

        if (item.icon != null)
            itemIcon.sprite = item.icon;

        buyButton.onClick.RemoveAllListeners();
        
        if (isOwned)
        {
            buyButtonText.text = "Owned";
            buyButton.interactable = false;
        }
        else
        {
            buyButtonText.text = "Buy";
            buyButton.interactable = true;
            buyButton.onClick.AddListener(OnBuyClick);
        }
    }

    void OnBuyClick()
    {
        _shopReference.BuyItem(_itemID);
    }
}
