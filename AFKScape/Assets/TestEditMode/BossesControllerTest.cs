using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

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
            bossesController = new BossesController();
            inventory = new Inventory();
            bank = new Bank();

            bossesController.Initialize(inventory, bank);
        }

        private bool CheckThreshold(float sim, float expected)
        {
            float diff = Mathf.Abs(sim - expected);
            float thresh = (20.0F / 100.0F) * expected;
            return diff < thresh;
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
            bossesController.BossGameLoop(monster, seconds/10);
        }

        [Test]
        public void ZulrahDropRateTest()
        {
            Monster monster = bossesController.bossesClasses["Zulrah"];
            Dictionary<long, BigInteger> dropTableDict = monster.GetDropTableDict();

            Dictionary<long, float> expectedRate = new Dictionary<long, float>();
            expectedRate[12921] = 1 / 4000.0F;
            expectedRate[1391] = (2 * 10 * 10) / 248.0F;
            expectedRate[13200] = 1 / 6553.0F;
            expectedRate[12922] = 1 / 512.0F;

            expectedRate[2366] = 2 / 7446.93F;
            expectedRate[1617] = 2 / 11286.76F;

            int iterations = 1_000_000;
            for (int i = 0; i < iterations; i++)
            {
                monster.KillBoss(dropTableDict);
            }

            foreach (long id in expectedRate.Keys)
            {
                float simRate = (float)dropTableDict[id] / iterations;
                Assert.IsTrue(CheckThreshold(simRate, expectedRate[id]), Database.items[id].name);
            }
        }

        [Test]
        public void VorkathDropRateTest()
        {
            Monster monster = bossesController.bossesClasses["Vorkath"];
            Dictionary<long, BigInteger> dropTableDict = monster.GetDropTableDict();

            Dictionary<long, float> expectedRate = new Dictionary<long, float>();
            expectedRate[22006] = 1 / 5000.0F;
            expectedRate[1305] = 4 / 150.0F;

            expectedRate[5300] = 2 / 112.3F;
            expectedRate[5118] = 2 / 625.0F;
            expectedRate[22924] = 2 / 3125.0F;

            expectedRate[1621] = 2 / 1536.0F;
            expectedRate[830] = 10 / 24576.0F;

            int iterations = 1_000_000;
            for (int i = 0; i < iterations; i++)
            {
                monster.KillBoss(dropTableDict);
            }

            foreach (long id in expectedRate.Keys)
            {
                float simRate = (float)dropTableDict[id] / iterations;
                Assert.IsTrue(CheckThreshold(simRate, expectedRate[id]) + 10, Database.items[id].name + ", " + simRate.ToString() + ", " + expectedRate[id].ToString());
            }
        }
    }
}
