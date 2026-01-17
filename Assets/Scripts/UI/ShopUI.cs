using System.Collections.Generic;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    public Transform container;
    public GameObject shopItemPrefab;

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

            uiScript.Setup(item, shopScript, isOwned);
        }
    }
}
