using System.Collections.Generic;
using UnityEngine;

public class CosmeticManager : MonoBehaviour
{
    public SkinnedMeshRenderer playerMesh;
    public Material defaultMaterial;
    public Transform hatSlot;

    public List<ShopItem> availableCosmetics;

    private GameObject currentHat;
    private Material currentMatSkin;
    private GameObject currentShoes;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
