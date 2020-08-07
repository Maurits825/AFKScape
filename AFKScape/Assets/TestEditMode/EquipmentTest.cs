using System.Collections;
using System.Collections.Generic;
using System.Numerics;
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
        private long ancestralHat = 21018;
        private long runeArrows = 937;
        private long bronzeArrows = 897;
        private long whip = 4151;
        private long avernic = 22322;
        private long arcane = 12825;
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
            inventory.AddItem(ancestralHat, 1);
            equipment.EquipItem(helmOfNeitz);
            Assert.IsFalse(inventory.Contains(helmOfNeitz));

            equipment.EquipItem(ancestralHat);
            Assert.IsFalse(inventory.Contains(ancestralHat));

            inventory.AddItem(avernic, 1);
            inventory.AddItem(arcane, 1);
            equipment.EquipItem(avernic);
            equipment.EquipItem(arcane);
            Assert.IsFalse(inventory.Contains(arcane));
        }

        [Test]
        public void UnEquipItemTest()
        {
            inventory.AddItem(helmOfNeitz, 1);
            equipment.EquipItem(helmOfNeitz);
            Assert.IsFalse(inventory.Contains(helmOfNeitz));

            equipment.UnEquipItem(helmOfNeitz);
            Assert.IsTrue(inventory.Contains(helmOfNeitz));

            inventory.AddItem(ags, 1);
            equipment.EquipItem(ags);
            Assert.IsFalse(inventory.Contains(ags));
            equipment.UnEquipItem(ags);
            Assert.IsTrue(inventory.Contains(ags));
        }

        [Test]
        public void TwoHandedTest()
        {
            inventory.AddItem(ags, 1);
            inventory.AddItem(whip, 1);
            inventory.AddItem(avernic, 1);

            equipment.EquipItem(whip);
            equipment.EquipItem(avernic);
            Assert.IsFalse(inventory.Contains(whip));
            Assert.IsFalse(inventory.Contains(avernic));
            Assert.IsTrue(inventory.Contains(ags));

            equipment.EquipItem(ags);
            Assert.IsTrue(inventory.Contains(whip));
            Assert.IsTrue(inventory.Contains(avernic));
            Assert.IsFalse(inventory.Contains(ags));

            equipment.EquipItem(avernic);
            Assert.IsTrue(inventory.Contains(whip));
            Assert.IsFalse(inventory.Contains(avernic));
            Assert.IsTrue(inventory.Contains(ags));

            equipment.EquipItem(ags);
            equipment.EquipItem(whip);
            Assert.IsFalse(inventory.Contains(whip));
            Assert.IsTrue(inventory.Contains(avernic));
            Assert.IsTrue(inventory.Contains(ags));
        }

        [Test]
        public void AmmoTest()
        {
            inventory.AddItem(runeArrows, 15);
            inventory.AddItem(bronzeArrows, 35);

            equipment.EquipItem(runeArrows);
            Assert.IsFalse(inventory.Contains(runeArrows));
            Assert.IsTrue(inventory.Contains(bronzeArrows));

            equipment.EquipItem(bronzeArrows);
            Assert.IsTrue(inventory.Contains(runeArrows));
            Assert.AreEqual((BigInteger)15, inventory.GetAmount(runeArrows));
            Assert.IsFalse(inventory.Contains(bronzeArrows));

            equipment.UnEquipItem(bronzeArrows);
            Assert.IsTrue(inventory.Contains(runeArrows));
            Assert.AreEqual((BigInteger)15, inventory.GetAmount(runeArrows));
            Assert.IsTrue(inventory.Contains(bronzeArrows));
            Assert.AreEqual((BigInteger)35, inventory.GetAmount(bronzeArrows));
        }

        [Test]
        public void TotalEquipmentStatsTest()
        {
            EquipmentStats stats;
            stats = equipment.GetTotalEquipmentStats();
            Assert.Zero(stats.attackSlash);
            Assert.Zero(stats.meleeStrength);

            inventory.AddItem(whip, 1);
            inventory.AddItem(avernic, 1);
            equipment.EquipItem(whip);
            stats = equipment.GetTotalEquipmentStats();
            Assert.AreEqual(82, stats.attackSlash);
            Assert.AreEqual(82, stats.meleeStrength);

            equipment.EquipItem(avernic);
            stats = equipment.GetTotalEquipmentStats();
            Assert.AreEqual(82 + 29, stats.attackSlash);
            Assert.AreEqual(82 + 8, stats.meleeStrength);

            equipment.UnEquipItem(whip);
            stats = equipment.GetTotalEquipmentStats();
            Assert.AreEqual(29, stats.attackSlash);
            Assert.AreEqual(8, stats.meleeStrength);

            equipment.UnEquipItem(avernic);
            stats = equipment.GetTotalEquipmentStats();
            Assert.AreEqual(0, stats.attackSlash);
            Assert.AreEqual(0, stats.meleeStrength);
        }
    }
}
