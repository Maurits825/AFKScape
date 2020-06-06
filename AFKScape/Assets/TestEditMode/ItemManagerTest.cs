using System.Collections;
using System.Collections.Generic;
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
    }
}
