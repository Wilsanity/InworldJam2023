using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlotManager : MonoBehaviour
{
    [SerializeField] private WeaponHolderSlot[] weaponSlots;

    public void LoadWeaponOnSlot(ItemObject item, int slot)
    {
        if (slot >= weaponSlots.Length)
            return;

        weaponSlots[slot].LoadWeaponModel(item);
    }
}
