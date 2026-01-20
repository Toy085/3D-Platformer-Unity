using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopUI : MonoBehaviour
{
    public Transform container;
    public GameObject shopItemPrefab;
    public TextMeshProUGUI coinText;

    private void Awake()
    {
        int slot = PlayerPrefs.GetInt("SelectedSlot", 1);
        PlayerData data = SaveSystem.LoadPlayer(slot);

        if(data != null)
            coinText.text = "Coins: " + data.coins.ToString();
        else
            coinText.text = "Coins: 0";
    }

    public void PopulateShop(List<ShopItem> items, Shop shopScript, PlayerData playerData)
    {
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }

        foreach (ShopItem item in items)
        {
            GameObject itemGO = Instantiate(shopItemPrefab, container);
            ShopItemUI uiScript = itemGO.GetComponent<ShopItemUI>();

            bool isOwned = playerData.cosmetics.Contains(item.id);
            bool isEquipped = playerData.equippedCosmetic == item.id;

            uiScript.Setup(item, shopScript, isOwned, isEquipped);
        }
        coinText.text = "Coins: " + playerData.coins.ToString();
    }
}
