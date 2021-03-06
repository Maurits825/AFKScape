﻿using System.Numerics;

public class Inventory : Storage
{
    public Inventory()
    {
        totalSlots = 28;
    }

    public override void RaiseItemAddedEvent(long id, BigInteger amount)
    {
        EventManager.Instance.InvItemAdded(id, amount);
    }

    public override void RaiseItemRemovedEvent(long id, BigInteger amount)
    {
        EventManager.Instance.InvItemRemoved(id, amount);
    }
}
