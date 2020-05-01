using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : Storage
{
    public Inventory()
    {
        totalSlots = 28;
    }

    public override void RaiseItemChangedEvent(long id, int amount)
    {
        EventManager.Instance.ItemChanged(id, amount);
    }

}
