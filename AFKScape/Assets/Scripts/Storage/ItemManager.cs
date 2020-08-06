using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class ItemManager
{
    private Inventory inventory;
    private Bank bank;
    private Equipment equipment;

    public void Initialize(Inventory inventory, Bank bank, Equipment equipment)
    {
        this.inventory = inventory;
        this.bank = bank;
        this.equipment = equipment;

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
                else
                {
                    equipment.EquipItem(id); ;
                }
                break;

            case Slot.State.Equipped:
                equipment.UnEquipItem(id);
                break;

            default:
                break;
        }
    }

    private void WithdrawToInventory(long id)
    {
        BigInteger amount;
        if (bank.amount == -1)
        {
            amount = bank.GetAmount(id);
        }
        else
        {
            amount = bank.amount;
        }

        BigInteger amountRemoved = bank.RemoveItem(id, amount);
        inventory.AddItem(id, amountRemoved);
    }

    private void DepositToBank(long id)
    {
        BigInteger amount;
        if (bank.amount == -1)
        {
            amount = inventory.GetAmount(id);
        }
        else
        {
            amount = bank.amount;
        }

        BigInteger amountRemoved = inventory.RemoveItem(id, amount);
        bank.AddItem(id, amountRemoved);
    }
}
