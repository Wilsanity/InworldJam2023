using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    public EquipmentSlot[] slots;

    public void Equip(ItemObject item)
    {
        foreach(var slot in slots)
        {
            if(slot.itemType == item.itemType)
            {
                slot.itemObject = item;
            }
        }
    }
}
