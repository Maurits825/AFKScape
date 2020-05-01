using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : Storage
{
    public Inventory()
    {
        totalSlots = 28;
    }

    public override void RaiseItemAddedEvent(long id, int amount, int amounDiff)
    {
        EventManager.Instance.InvItemAdded(id, amount);
    }

    public override void RaiseItemRemovedEvent(long id, int amount, int amounDiff)
    {
        EventManager.Instance.InvItemRemoved(id, amount);
    }
}
