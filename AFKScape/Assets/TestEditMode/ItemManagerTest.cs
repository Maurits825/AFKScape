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
        private Equipment equipment;

        private long dragonClaws = 13652;
        private long whip = 4151;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            Database.LoadItems();
            Database.LoadIcons();
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            Database.items.Clear();
            Database.sprites.Clear();
        }

        [SetUp]
        public void SetUp()
        {
            EventManager.SetIntance(new EventManager());
            itemManager = new ItemManager();
            inventory = new Inventory();
            bank = new Bank();
            equipment = new Equipment();

            itemManager.Initialize(inventory, bank, equipment);
            equipment.Initialize(inventory);
        }

        [Test]
        public void SlotClickedInInventoryTest()
        {
            inventory.AddItem(dragonClaws, 1);
            inventory.AddItem(whip, 1);
            bank.isActive = true;
            EventManager.Instance.SlotClicked(Slot.State.Inventory, dragonClaws);
            Assert.IsFalse(inventory.Contains(dragonClaws));
            Assert.IsTrue(bank.Contains(dragonClaws));

            bank.isActive = false;
            EventManager.Instance.SlotClicked(Slot.State.Inventory, whip);
            Assert.IsFalse(inventory.Contains(whip));
            Assert.IsFalse(bank.Contains(whip));
            //TODO check whip equipped, maybe check cmb bonuses/stats?
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
