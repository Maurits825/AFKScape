using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class ItemManagerTest
    {
        private ItemManager itemManager;
        private Inventory inventory;
        private Bank bank;

        private long dragonClaws = 13652;

        [SetUp]
        public void SetUp()
        {
            EventManager.SetIntance(new EventManager());
            itemManager = new ItemManager();
            inventory = new Inventory();
            bank = new Bank();

            itemManager.Initialize(inventory, bank);
        }

        [Test]
        public void SlotClickedInInventoryTest()
        {
            inventory.AddItem(dragonClaws, 1);
            bank.isActive = false;
            EventManager.Instance.SlotClicked(Slot.State.Inventory, dragonClaws);
            Assert.IsTrue(inventory.Contains(dragonClaws));
            Assert.IsFalse(bank.Contains(dragonClaws));

            bank.isActive = true;
            EventManager.Instance.SlotClicked(Slot.State.Inventory, dragonClaws);
            Assert.IsFalse(inventory.Contains(dragonClaws));
            Assert.IsTrue(bank.Contains(dragonClaws));
        }

        [Test]
        public void SlotClickedInBankTest()
        {
            bank.isActive = true;
            bank.AddItem(dragonClaws, 1);            
            EventManager.Instance.SlotClicked(Slot.State.Bank, dragonClaws);
            Assert.IsTrue(inventory.Contains(dragonClaws));
            Assert.IsFalse(bank.Contains(dragonClaws));
        }

        [Test]
        public void BankAmountTest()
        {
            bank.isActive = true;
            BigInteger amount = 55;
            bank.AddItem(dragonClaws, amount);
            bank.amount = -1;

            EventManager.Instance.SlotClicked(Slot.State.Bank, dragonClaws);
            Assert.IsTrue(inventory.Contains(dragonClaws));
            Assert.AreEqual(amount, inventory.GetAmount(dragonClaws));
            Assert.IsFalse(bank.Contains(dragonClaws));

            EventManager.Instance.SlotClicked(Slot.State.Inventory, dragonClaws);
            Assert.IsTrue(bank.Contains(dragonClaws));
            Assert.AreEqual(amount, bank.GetAmount(dragonClaws));
            Assert.IsFalse(inventory.Contains(dragonClaws));
        }
    }
}
