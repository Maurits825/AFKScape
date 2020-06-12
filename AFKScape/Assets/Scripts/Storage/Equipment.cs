using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment
{
    //TODO fix py scrip for items, make the slot a int, it then get auto-converted to enum
    public enum EquipmentSlot
    {
        head = 0,
        cape,
        neck,
        ammo,
        weapon,
        body,
        shield,
        legs,
        hands,
        feet,
        ring,
        twoHanded,
    }

    private List<long> equipedItems = new List<long>();

    public void EquipItem(long id, EquipmentSlot slot)
    {
        int slotIndex = (int)slot;

        if (equipedItems[slotIndex] != 0)
        {
            //unequip item
        }

        equipedItems[slotIndex] = id;
        EventManager.Instance.ItemEquipped(id, slot);
    }
}
