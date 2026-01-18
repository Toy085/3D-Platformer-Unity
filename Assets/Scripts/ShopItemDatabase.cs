using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "NewShopDatabase", menuName = "Shop/Database")]
public class ShopItemDatabase : ScriptableObject
{
    public List<ShopItem> allItems;

    public ShopItem GetItemByID(int id)
    {
        return allItems.Find(item => item.id == id);
    }
}