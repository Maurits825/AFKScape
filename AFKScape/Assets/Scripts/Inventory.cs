﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public Dictionary<long, int> items = new Dictionary<long, int>(); //id and amount, add slot num here?
    private int usedSlots = 0;
    private int totalSlots;

    public Inventory(int slots)
    {
        totalSlots = slots;
    }

    public void AddItem(long id, int amount)
    {
        if (items.ContainsKey(id))
        {
            items[id] += amount;
        }
        else if (usedSlots < totalSlots)
        {
            items.Add(id, amount);
            usedSlots++;
        }
            
    }

    public void RemoveItem(long id, int amount)
    {
        if (items.ContainsKey(id))
        {
            items[id] -= amount;
            usedSlots--;

            if (items[id] < 0)
            {
                items[id] = 0;
            }
        }

    }
}
