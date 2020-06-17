using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class EquipmentTest
    {
        private Inventory inventory;
        private Equipment equipment;

        private long helmOfNeitz = 10828;
        private long runeArrows = 937;
        private long whip = 4151;
        private long avernic = 22322;
        private long ags = 11802;

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
            inventory = new Inventory();
            equipment = new Equipment();

            equipment.Initialize(inventory);
        }

        [Test]
        public void EquipItemTest()
        {
            inventory.AddItem(helmOfNeitz, 1);
            equipment.EquipItem(helmOfNeitz, Equipment.EquipmentSlot.head);
            Assert.IsFalse(inventory.Contains(helmOfNeitz));
            //TODO check is equipped
        }

        [Test]
        public void UnEquipItemTest()
        {
            inventory.AddItem(helmOfNeitz, 1);
            equipment.EquipItem(helmOfNeitz, Equipment.EquipmentSlot.head);
            Assert.IsFalse(inventory.Contains(helmOfNeitz));
            //TODO check is equipped

            equipment.UnEquipItem(helmOfNeitz, Equipment.EquipmentSlot.head);
            Assert.IsTrue(inventory.Contains(helmOfNeitz));
        }

        [Test]
        public void TwoHandedTest()
        {
            inventory.AddItem(ags, 1);
            inventory.AddItem(whip, 1);
            inventory.AddItem(avernic, 1);

            equipment.EquipItem(whip, Equipment.EquipmentSlot.weapon);
            equipment.EquipItem(avernic, Equipment.EquipmentSlot.shield);
            Assert.IsFalse(inventory.Contains(whip));
            Assert.IsFalse(inventory.Contains(avernic));
            Assert.IsTrue(inventory.Contains(ags));
            //TODO check whip/def equipped

            equipment.EquipItem(ags, Equipment.EquipmentSlot.twoHanded);
            Assert.IsTrue(inventory.Contains(whip));
            Assert.IsTrue(inventory.Contains(avernic));
            Assert.IsFalse(inventory.Contains(ags));

            equipment.EquipItem(avernic, Equipment.EquipmentSlot.shield);
            Assert.IsTrue(inventory.Contains(whip));
            Assert.IsFalse(inventory.Contains(avernic));
            Assert.IsTrue(inventory.Contains(ags));

            equipment.EquipItem(ags, Equipment.EquipmentSlot.twoHanded);
            equipment.EquipItem(whip, Equipment.EquipmentSlot.weapon);
            Assert.IsFalse(inventory.Contains(whip));
            Assert.IsTrue(inventory.Contains(avernic));
            Assert.IsTrue(inventory.Contains(ags));
        }

        [Test]
        public void AmmoTest()
        {
            //TODO!
        }
    }
}
