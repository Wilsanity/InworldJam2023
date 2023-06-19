using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryItemDisplay : MonoBehaviour, IPointerClickHandler
{
    public ItemObject item;

    public TextMeshProUGUI nameText;
    public Image itemIcon;

    public void OnPointerClick(PointerEventData eventData)
    {
        GetComponentInParent<Equipment>().Equip(item);
    }
}
