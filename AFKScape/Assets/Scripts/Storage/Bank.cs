using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bank : Storage
{
    public Bank()
    {
        totalSlots = 5000;
    }

    public override void RaiseItemAddedEvent(long id, int amount, int amounDiff)
    {
        EventManager.Instance.BankItemAdded(id, amount, amounDiff);
    }

    public override void RaiseItemRemovedEvent(long id, int amount, int amounDiff)
    {
        EventManager.Instance.BankItemRemoved(id, amount, amounDiff);
    }
}
