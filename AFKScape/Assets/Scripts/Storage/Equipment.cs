using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class Equipment
{
    private Inventory inventory;

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

    private long[] equipedItems = new long[11];

    public void Initialize(Inventory inventory)
    {
        this.inventory = inventory;
    }

    //TODO ammo slot count and 2h
    public void EquipItem(long id, EquipmentSlot slot)
    {
        if (Database.items[id].equipable && CheckRequirements())
        {
            int slotIndex = (int)slot;

            BigInteger amountRemoved = inventory.RemoveItem(id, 1);

            if (equipedItems[slotIndex] != 0)
            {
                UnEquipItem(equipedItems[slotIndex], slot);
            }

            equipedItems[slotIndex] = id;

            EventManager.Instance.ItemEquipped(id, slot);
        }
    }

    public void UnEquipItem(long id, EquipmentSlot slot)
    {
        int slotIndex = (int)slot;
        equipedItems[slotIndex] = 0;
        inventory.AddItem(id, 1);

        EventManager.Instance.ItemUnEquipped(id, slot);
    }

    private bool CheckRequirements()
    {
        //TODO implement
        return true;
    }
}
