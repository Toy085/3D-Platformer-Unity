using System.Collections.Generic;
using UnityEngine;

public class CosmeticManager : MonoBehaviour
{
    public MeshRenderer playerMesh;
    public Material defaultMaterial;
    public Transform hatSlot;
    public Transform leftShoeSlot;
    public Transform rightShoeSlot;

    public List<ShopItem> availableCosmetics;

    private GameObject currentHat;
    private Material currentMatSkin;
    private GameObject currentShoes;
    
    private void Awake()
    {
        if (playerMesh != null)
        {
            playerMesh.material = defaultMaterial;
        }
    }

    private void Start()
    {
        int slot = PlayerPrefs.GetInt("SelectedSlot", 1);
        PlayerData data = SaveSystem.LoadPlayer(slot);

        if (data != null && data.equippedCosmetic != -1)
        {
            ApplyCosmetic(data.equippedCosmetic);
        } else 
        {
            ResetToDefault();
        }
    }

    public void ApplyCosmetic(int cosmeticID)
    {
        ShopItem cosmetic = availableCosmetics.Find(item => item.id == cosmeticID);
        if (cosmetic != null)
        {
            switch (cosmetic.cosmeticType)
            {
                case 1: // Hat
                    if (currentHat != null)
                        Destroy(currentHat);
                    currentHat = Instantiate(cosmetic.itemPrefab, hatSlot);
                    break;
                case 2: // Mat Skin
                    if (playerMesh != null)
                    {
                        currentMatSkin = cosmetic.itemPrefab.GetComponent<Renderer>().sharedMaterial;
                        playerMesh.material = currentMatSkin;
                    }
                    break;
                case 3: // Shoes
                    if (currentShoes != null)
                        Destroy(currentShoes);
                    currentShoes = Instantiate(cosmetic.itemPrefab, leftShoeSlot);
                    Instantiate(cosmetic.itemPrefab, rightShoeSlot);
                    break;
                default:
                    Debug.LogWarning("Unknown cosmetic type.");
                    break;
            }
        }
    }

    public void EquipOwnedItem(int itemID)
    {
        int slot = PlayerPrefs.GetInt("SelectedSlot", 1);
        PlayerData data = SaveSystem.LoadPlayer(slot);

        if (data.cosmetics.Contains(itemID))
        {
            data.equippedCosmetic = itemID;
            SaveSystem.SavePlayer(data, slot);
            
            ApplyCosmetic(itemID);
        }
    }

    public void ResetToDefault()
    {
        if (currentHat != null) Destroy(currentHat);
        if (currentShoes != null) Destroy(currentShoes);
        if (playerMesh != null) playerMesh.material = defaultMaterial;
    }
}
