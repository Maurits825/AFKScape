using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager
{
    private Inventory inventory;
    private Bank bank;

    public void Initialize(Inventory inventory, Bank bank)
    {
        this.inventory = inventory;
        this.bank = bank;

        EventManager.Instance.OnSlotClicked += HandleSlotClicked;
    }

    private void HandleSlotClicked(Slot.State state, long id)
    {
        switch (state)
        {
            case Slot.State.Bank:
                WithdrawToInventory(id);
                break;

            case Slot.State.Inventory:
                if (bank.isActive)
                {
                    DepositToBank(id);
                }
                break;

            case Slot.State.Equipped:
                break;

            default:
                break;
        }
    }

    private void WithdrawToInventory(long id)
    {
        bank.RemoveItem(id, bank.amount);
        inventory.AddItem(id, bank.amount);
    }

    private void DepositToBank(long id)
    {
        inventory.RemoveItem(id, bank.amount);
        bank.AddItem(id, bank.amount);
    }
}
