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
    
    public List<ShopItem> itemsForSale;

    [Header("Prompt Sprites")]
    public Sprite keyboardSprite;
    public Sprite gamepadSprite;

    private Image promptImage;
    private PlayerInput playerInput;
    private bool shopOpen = false;
    private PlayerData playerData;
    private ShopUI shopUIHandler;

    void Awake()
    {
        promptImage = promptUI.GetComponent<Image>();
        playerInput = player.GetComponent<PlayerInput>();
        shopUIHandler = shopUI.GetComponent<ShopUI>();
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

        int slot = PlayerPrefs.GetInt("SelectedSlot", 1); 
        playerData = SaveSystem.LoadPlayer(slot);

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
        if (shopUIHandler != null)
        {
            shopUIHandler.PopulateShop(itemsForSale, this, playerData);
        }
        else
        {
            Debug.LogError("ShopUI component not found on the shopUI GameObject!");
        }
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

        int slot = PlayerPrefs.GetInt("SelectedSlot", 1);
        SaveSystem.SavePlayer(playerData, slot);

        PopulateShopItems();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + Offset, interactDistance);
    }
}
