using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Shop : MonoBehaviour
{
    [Header("Shop Settings")]
    public Vector3 Offset;
    public GameObject shopUI;
    public Transform player;
    public float interactDistance = 3f;
    public GameObject promptUI;
    public PlayerData playerData;
    public List<ShopItem> itemsForSale;

    [Header("Prompt Sprites")]
    public Sprite keyboardSprite;
    public Sprite gamepadSprite;

    private Image promptImage;
    private PlayerInput playerInput;
    private bool shopOpen = false;

    void Awake()
    {
        promptImage = promptUI.GetComponent<Image>();
        playerInput = player.GetComponent<PlayerInput>();
    }

    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position + Offset);
        bool isNear = distance <= interactDistance && !shopOpen;

        promptUI.SetActive(isNear);
        if (isNear) UpdatePromptIcon();
    }
    
    public void TryOpenShop()
    {
        if (shopOpen)
            return;

        float distance = Vector3.Distance(player.position, transform.position + Offset);

        if (distance <= interactDistance)
        {
            OpenShop();
        }
    }

    void UpdatePromptIcon()
    {
        if (playerInput.currentControlScheme == "Keyboard&Mouse")
        {
            promptImage.sprite = keyboardSprite;
        }
        else if (playerInput.currentControlScheme == "Gamepad")
        {
            promptImage.sprite = gamepadSprite;
        }
    }

    public void OpenShop()
    {
        shopOpen = true;

        shopUI.SetActive(true);
        PopulateShopItems();
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void CloseShop()
    {
        shopOpen = false;

        shopUI.SetActive(false);
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void PopulateShopItems()
    {
        /*ShopUI shopUIComponent = shopUI.GetComponent<ShopUI>();
        shopUIComponent.ClearItems();

        foreach (ShopItem item in itemsForSale)
        {
            shopUIComponent.AddItem(item);
        }*/
    }

    public void BuyItem(int itemID)
    {
        ShopItem item = itemsForSale.Find(i => i.id == itemID);
        if (item == null) return;

        if (item.type != 0) return;

        if (playerData.coins < item.price) return;
        if (playerData.cosmetics.Contains(itemID)) return;

        playerData.coins -= item.price;
        playerData.cosmetics.Add(itemID);

        SaveSystem.SavePlayer(playerData, 1);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + Offset, interactDistance);
    }
}
