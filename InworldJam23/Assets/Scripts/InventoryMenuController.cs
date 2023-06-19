using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryMenuController : MonoBehaviour
{
    public GameObject inventoryUIGameObject;
    public WeaponSlotManager weaponSlotManager;

    public List<InventoryItemDisplay> itemDisplays;
    public List<ItemObject> inventoryList;

    public CameraController cameraController;

    public ItemObject item;

    private Controls controls;

    private Controls Controls
    {
        get
        {
            if (controls != null) { return controls; }
            return controls = new Controls();
        }
    }


    public void Start()
    {
        enabled = true;

        Controls.Player.Inventory.performed += ctx => OpenInventory();

        weaponSlotManager.LoadWeaponOnSlot(item, 0);
        weaponSlotManager.LoadWeaponOnSlot(item, 1);
    }

    private void OnEnable() => Controls.Enable();
    private void OnDisable() => Controls.Disable();

    void OpenInventory()
    {
        if (inventoryUIGameObject.activeSelf)
        {
            inventoryUIGameObject.SetActive(false);
            cameraController.enabled = true;

            return;
        }

        cameraController.enabled = false;
        ShowInventoryUI();
    }

    public void ShowInventoryUI()
    {
        inventoryUIGameObject.SetActive(true);

        /*
        foreach(ItemSlot slot in itemSlots)
        {
            slot.image.enabled = false;
            slot.gameObject.SetActive(false);
        }

        SetEquipmentUI();
        SetItemSlotUI();
        */
    }

    void SetItemSlotUI()
    {
        /*
        for (int i = 0; i < inventorySize; i++)
        {
            itemSlots[i].gameObject.SetActive(true);
            itemSlots[i].image.enabled = false;
        }

        int counter = 0;
        foreach (itemObject item in inventoryList)
        {
            itemSlots[counter].image.enabled = true;
            itemSlots[counter].image.sprite = card.cardSprite;
        }
        */
    }
}
