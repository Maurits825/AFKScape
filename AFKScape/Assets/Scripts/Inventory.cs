using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public Dictionary<long, ItemSlot> items = new Dictionary<long, ItemSlot>(); //id and amount, add slot num here?

    private int nextAvailableSlot = 0; //TODO handle this better
    private int usedSlots = 0;
    private int totalSlots;

    public class ItemSlot
    {
        public int amount;
        public int slotIndex;

        public ItemSlot(int num, int idx)
        {
            amount = num;
            slotIndex = idx;
        }
    }

    public Inventory(int slots)
    {
        totalSlots = slots;
    }

    public bool AddItem(long id, int amount)
    {
        bool addedItem;

        if (items.ContainsKey(id))
        {
            items[id].amount += amount;
            addedItem = true;
        }
        else if (usedSlots < totalSlots)
        {
            items.Add(id, new ItemSlot(amount, nextAvailableSlot));
            nextAvailableSlot++;
            usedSlots++;
            addedItem = true;
        }
        else
        {
            addedItem = false;
        }

        if (addedItem)
        {
            EventManager.Instance.ItemChanged(id, items[id].amount, items[id].slotIndex);
        }

        return addedItem;
            
    }

    public bool RemoveItem(long id, int amount)
    {
        bool removedItem;

        if (items.ContainsKey(id))
        {
            items[id].amount -= amount;
            removedItem = true;



            if (items[id].amount < 0)
            {
                items.Remove(id);
                nextAvailableSlot--;
                usedSlots--;

            }
        }
        else
        {
            removedItem = false;
        }

        if (removedItem)
        {
            EventManager.Instance.ItemChanged(id, items[id].amount, items[id].slotIndex);
        }

        return removedItem;

    }
}
