using NUnit.Framework;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

namespace Tests
{
    public class BossesControllerTest
    {
        private BossesController bossesController;
        private Inventory inventory;
        private Bank bank;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            Database.LoadBosses();
            if (Database.items.Count != 0)
            {
                Database.items.Clear();
            }
            Database.LoadItems();
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            Database.bossesNames.Clear();
            Database.items.Clear();
        }

        [SetUp]
        public void SetUp()
        {
            EventManager.SetIntance(new EventManager());
            inventory = new Inventory();
            bank = new Bank();

            bossesController = new BossesController(inventory, bank);
        }

        [Test]
        public void UnityEngineRandomRangeTest()
        {
            int iterations = 1_000_000;
            int value;
            int baseChance = 5;
            int count = 0;
            for (int i = 0; i < iterations; i++)
            {
                value = UnityEngine.Random.Range(1, baseChance);
                if (value == baseChance)
                {
                    count++;
                }
            }

            Assert.AreEqual(0, count);
        }

        [Test]
        public void BossGameLoopTest()
        {
            Monster monster = bossesController.bossesClasses["Zulrah"];
            bossesController.OnBossSelected("Zulrah");

            int days = 1;
            int seconds = days * 86400;
            bossesController.BossGameLoop(monster, seconds / 10);
        }
    }
}
