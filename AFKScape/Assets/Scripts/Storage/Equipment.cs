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
    private bool isTwoHandedEquipped = false;
    private BigInteger ammoCount = 0;

    public void Initialize(Inventory inventory)
    {
        this.inventory = inventory;
    }

    //TODO button to show item stats UI and keep track of item stats/dmg bonuses
    public void EquipItem(long id, EquipmentSlot slot)
    {
        if (Database.items[id].equipableByPlayer && CheckRequirements())
        {
            int slotIndex;
            EquipmentSlot actualSlot;
            if (slot == EquipmentSlot.twoHanded)
            {
                slotIndex = (int)EquipmentSlot.weapon;
                actualSlot = EquipmentSlot.weapon;
            }
            else
            {
                slotIndex = (int)slot;
                actualSlot = slot;
            }

            BigInteger amount;
            if (slot == EquipmentSlot.ammo)
            {
                amount = inventory.RemoveItem(id, inventory.GetAmount(id));
                ammoCount = amount;
            }
            else
            {
                amount = inventory.RemoveItem(id, 1);
            }

            if (slot == EquipmentSlot.twoHanded)
            {
                if (equipedItems[(int)EquipmentSlot.weapon] != 0)
                {
                    UnEquipItem(equipedItems[(int)EquipmentSlot.weapon], EquipmentSlot.weapon);
                }

                if (equipedItems[(int)EquipmentSlot.shield] != 0)
                {
                    UnEquipItem(equipedItems[(int)EquipmentSlot.shield], EquipmentSlot.shield);
                }

                isTwoHandedEquipped = true;
            }
            else if (slot == EquipmentSlot.shield)
            {
                if (equipedItems[(int)EquipmentSlot.shield] != 0)
                {
                    UnEquipItem(equipedItems[(int)EquipmentSlot.shield], EquipmentSlot.shield);
                }
                else if (isTwoHandedEquipped)
                {
                    UnEquipItem(equipedItems[(int)EquipmentSlot.weapon], EquipmentSlot.weapon);
                }

                isTwoHandedEquipped = false;
            }
            else if (slot == EquipmentSlot.weapon)
            {
                if (equipedItems[(int)EquipmentSlot.weapon] != 0)
                {
                    UnEquipItem(equipedItems[(int)EquipmentSlot.weapon], EquipmentSlot.weapon);
                }

                isTwoHandedEquipped = false;
            }
            else if (equipedItems[slotIndex] != 0)
            {
                UnEquipItem(equipedItems[slotIndex], slot);
            }

            equipedItems[slotIndex] = id;
            EventManager.Instance.ItemEquipped(id, actualSlot, amount);
        }
    }

    public void UnEquipItem(long id, EquipmentSlot slot)
    {
        int slotIndex;
        if (slot == EquipmentSlot.twoHanded)
        {
            slotIndex = (int)EquipmentSlot.weapon;
        }
        else
        {
            slotIndex = (int)slot;
        }

        equipedItems[slotIndex] = 0;

        if (slot == EquipmentSlot.ammo)
        {
            inventory.AddItem(id, ammoCount);
            ammoCount = 0;
        }
        else
        {
            inventory.AddItem(id, 1);
        }

        EventManager.Instance.ItemUnEquipped(id, slot);
    }

    private bool CheckRequirements()
    {
        //TODO implement
        return true;
    }
}
