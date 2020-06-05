using System.Collections.Generic;
using System.Linq;
using System.Numerics;

public class Storage
{
    public Dictionary<long, BigInteger> items = new Dictionary<long, BigInteger>();

    private int usedSlots = 0;
    public int totalSlots;

    public bool Contains(long id)
    {
        return items.ContainsKey(id);
    }

    public bool AddItem(long id, BigInteger amount)
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

    public void AddMultipleItems(Dictionary<long, BigInteger> items)
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

    public bool RemoveItem(long id, BigInteger amount)
    {
        bool removedItem;

        if (items.ContainsKey(id))
        {
            BigInteger amountRemoved;
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
        List<BigInteger> amounts = new List<BigInteger>();
        foreach (KeyValuePair<long, BigInteger> item in items)
        {
            ids.Add(item.Key);
            amounts.Add(item.Value);
        }

        for (int i = 0; i < ids.Count; i++)
        {
            RemoveItem(ids[i], amounts[i]);
        }
    }

    public virtual void RaiseItemAddedEvent(long id, BigInteger amount, BigInteger amounDiff)
    {
    }

    public virtual void RaiseItemRemovedEvent(long id, BigInteger amount, BigInteger amounDiff)
    {
    }
}
