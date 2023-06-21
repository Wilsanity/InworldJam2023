using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ItemType 
{ 
    None = 0,
    Head = 1,
    Body = 2,
    Legs = 3,
    Boots = 4,
    MainHand = 5,
    OffHand = 6,
    Ring = 7,
    Necklace = 8,
    Earrings = 9
}

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class ItemObject : ScriptableObject
{
    public ItemType itemType;
    public Image itemIcon;

    public GameObject itemModel;
    public string attackAnimationString;
    //stats bonus
}
