using UnityEngine;

[System.Serializable]
public class ShopItem
{
    public int id;
    public int type; // 0 = Cosmetic, 1 = Ability
    public string itemName;
    public int price;
    public GameObject itemPrefab;
}
