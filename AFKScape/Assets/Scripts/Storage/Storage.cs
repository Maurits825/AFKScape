using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Storage
{
    public Dictionary<long, int> items = new Dictionary<long, int>();

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
            items[id] += amount;
            addedItem = true;
        }
        else if (usedSlots < totalSlots)
        {
            items.Add(id, amount);
            usedSlots++;
            addedItem = true;
        }
        else
        {
            addedItem = false;
        }

        if (addedItem)
        {
            RaiseItemAddedEvent(id, items[id], amount);
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
            int amountRemoved;
            if (amount > items[id])
            {
                items[id] = 0;
                amountRemoved = items[id];
            }
            else
            {
                items[id] -= amount;
                amountRemoved = amount;
            }

            removedItem = true;
            RaiseItemRemovedEvent(id, items[id], amountRemoved);

            if (items[id] == 0)
            {
                items.Remove(id);
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
        foreach (KeyValuePair<long, int> item in items)
        {
            ids.Add(item.Key);
            amounts.Add(item.Value);
        }

        for (int i = 0; i < ids.Count; i++)
        {
            RemoveItem(ids[i], amounts[i]);
        }
    }

    //TODO dont really need two events?
    public virtual void RaiseItemAddedEvent(long id, int amount, int amounDiff)
    {
    }

    public virtual void RaiseItemRemovedEvent(long id, int amount, int amounDiff)
    {
    }
}
