using UnityEngine;

[System.Serializable]
public class ShopItem
{
    public int id;
    public int type; // 0 = Cosmetic, 1 = Ability
    public int cosmeticType; // 0 = None, 1= Hat, 2 = Mat Skin, 3 = Shoes
    public string itemName;
    public int price;
    public GameObject itemPrefab;
}
