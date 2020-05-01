using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bank : Storage
{
    public Bank()
    {
        totalSlots = 5000;
    }

    public override void RaiseItemChangedEvent(long id, int amount)
    {
        EventManager.Instance.BankItemAdded(id, amount);
    }
}
