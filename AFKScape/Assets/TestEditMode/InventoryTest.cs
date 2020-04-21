using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;


namespace Tests
{
    public class InventoryTest
    {
        private readonly int inventorySlots = 28;
        private static Inventory inventory;
        private long heronId = 13320;
        private long dragonMedId = 1149;

        [SetUp]

        public void Setup()
        {
            inventory = new Inventory(inventorySlots);
            inventory.AddItem(heronId, 1);
            inventory.AddItem(heronId, 1);
            inventory.AddItem(dragonMedId, 8);
        }

        [Test]
        public void TestInventoryAdd()
        {
            Assert.AreEqual(true, inventory.Contains(heronId));
            Assert.AreEqual(true, inventory.Contains(dragonMedId));
        }

        [Test]
        public void TestInventoryRemove()
        {
            inventory.RemoveItem(heronId, 1);
            Assert.AreEqual(true, inventory.Contains(heronId));
            inventory.RemoveItem(heronId, 1);
            Assert.AreEqual(false, inventory.Contains(heronId));
            inventory.RemoveAll();
            Assert.AreEqual(false, inventory.Contains(dragonMedId));
        }
    }
}
