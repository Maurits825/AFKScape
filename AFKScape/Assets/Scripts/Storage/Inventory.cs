﻿using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class Inventory : Storage
{
    public Inventory()
    {
        totalSlots = 28;
    }

    public override void RaiseItemAddedEvent(long id, BigInteger amount, BigInteger amounDiff)
    {
        EventManager.Instance.InvItemAdded(id, amount);
    }

    public override void RaiseItemRemovedEvent(long id, BigInteger amount, BigInteger amounDiff)
    {
        EventManager.Instance.InvItemRemoved(id, amount);
    }
}
