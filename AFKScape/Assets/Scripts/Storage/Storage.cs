using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

public class Storage
{
    public Dictionary<long, ItemSlot> items = new Dictionary<long, ItemSlot>();

    private int nextAvailableSlot = 0;
    private int usedSlots = 0;
    public int totalSlots;

    public bool Contains(long id)
    {
        return items.ContainsKey(id);
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
            RaiseItemChangedEvent(id, items[id].amount, items[id].slotIndex);
        }

        return addedItem;

    }

    public void AddMultipleItems(Dictionary<long, int> items)
    {
        foreach (long id in items.Keys.ToList())
        {
            if (items[id] > 0)
            {
                AddItem(id, items[id]);
                items[id] = 0;
            }
        }
    }

    public bool RemoveItem(long id, int amount)
    {
        bool removedItem;

        if (items.ContainsKey(id))
        {
            items[id].amount -= amount;
            removedItem = true;

            RaiseItemChangedEvent(id, items[id].amount, items[id].slotIndex);

            if (items[id].amount <= 0)
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

        return removedItem;
    }

    public void RemoveAll()
    {
        List<long> ids = new List<long>();
        List<int> amounts = new List<int>();
        foreach (KeyValuePair<long, ItemSlot> item in items)
        {
            ids.Add(item.Key);
            amounts.Add(item.Value.amount);
        }

        for (int i = 0; i < ids.Count; i++)
        {
            RemoveItem(ids[i], amounts[i]);
        }
    }

    //TODO add event for add and remove item, will be needed when withdrawing and deposit items.
    public virtual void RaiseItemChangedEvent(long id, int amount, int slotIndex)
    {
    }
}
