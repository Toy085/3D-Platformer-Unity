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

    public void Setup(ShopItem item, Shop shopScript, bool isOwned, bool isEquipped)
    {
        if (item == null) return;

        itemNameText.text = item.itemName;
        itemPriceText.text = item.price.ToString();

        _itemID = item.id;
        _shopReference = shopScript;

        if (item.icon != null)
            itemIcon.sprite = item.icon;

        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(OnBuyClick);
        
        if (isEquipped)
        {
            buyButtonText.text = "Equipped";
            buyButton.interactable = false;
        }
        else if (isOwned)
        {
            buyButtonText.text = "Equip";
            buyButton.interactable = true;
        }
        else
        {
            buyButtonText.text = "Buy";
            buyButton.interactable = true;
        }
    }

    void OnBuyClick()
    {
        _shopReference.BuyItem(_itemID);
    }
}
