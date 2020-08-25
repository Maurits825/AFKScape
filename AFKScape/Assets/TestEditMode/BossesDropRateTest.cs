using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;

namespace Tests
{
    public class BossesDropRateTest
    {
        private BossesController bossesController;
        private Inventory inventory;
        private Bank bank;

        private const int Iterations = 500_000;
        private const float PercentThreshold = 20.0F;

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
            float thresh = (PercentThreshold / 100.0F) * expected;
            return diff < thresh;
        }

        private bool CheckAllRates(Monster monster, Dictionary<long, float> expectedRate)
        {
            bool passed = false;
            for (int attempt = 0; attempt < 5; attempt++)
            {
                passed = true;

                Dictionary<long, BigInteger> dropTableDict = monster.GetDropTableDict();
                foreach (long id in dropTableDict.Keys.ToList())
                {
                    if (dropTableDict[id] > 0)
                    {
                        dropTableDict[id] = 0;
                    }
                }

                for (int i = 0; i < Iterations; i++)
                {
                    monster.KillBoss(dropTableDict);
                }

                foreach (long id in expectedRate.Keys)
                {
                    float simRate = (float)dropTableDict[id] / Iterations;
                    bool result = (CheckThreshold(simRate, expectedRate[id]));
                    Assert.IsTrue(result, "Item: " + Database.items[id].name +
                        ", Expected: " + expectedRate[id].ToString() +
                        ", Sim: " + simRate);

                    if (!result)
                    {
                        passed = false;
                        break;
                    }
                }

                if (passed)
                {
                    break;
                }
            }

            return passed;
        }

        [Test]
        public void ZulrahDropRateTest()
        {
            Monster monster = bossesController.bossesClasses["Zulrah"];

            Dictionary<long, float> expectedRate = new Dictionary<long, float>();
            expectedRate[12921] = 1 / 4000.0F;
            expectedRate[1391] = (2 * 10 * 10) / 248.0F;
            expectedRate[13200] = 1 / 6553.0F;
            expectedRate[12922] = 1 / 512.0F;

            expectedRate[2366] = 2 / 7446.93F;
            expectedRate[1617] = 2 / 11286.76F;

            Assert.IsTrue(CheckAllRates(monster, expectedRate));
        }

        [Test]
        public void VorkathDropRateTest()
        {
            Monster monster = bossesController.bossesClasses["Vorkath"];

            Dictionary<long, float> expectedRate = new Dictionary<long, float>();
            expectedRate[22006] = 1 / 5000.0F;
            expectedRate[1305] = 4 / 150.0F;

            expectedRate[5300] = 2 / 112.3F;
            expectedRate[5118] = 2 / 625.0F;
            expectedRate[22924] = 2 / 3125.0F;

            expectedRate[1621] = 2 / 1536.0F;
            expectedRate[830] = 10 / 24576.0F;

            Assert.IsTrue(CheckAllRates(monster, expectedRate));
        }

        [Test]
        public void ClueMediumDropRateTest()
        {
            Monster monster = new Monster(Database.cluesNames[2]);
            monster.Initialize();

            Dictionary<long, float> expectedRate = new Dictionary<long, float>();
            expectedRate[2577] = 1 / 1133.0F;
            expectedRate[20266] = 1 / 2266.0F;

            expectedRate[1161] = 1 / 34.1F;
            expectedRate[855] = 1 / 37.9F;
            expectedRate[10364] = 1 / 341.0F;

            expectedRate[7329] = 7 / 189.4F;
            expectedRate[20275] = 1 / 341.0F;
            expectedRate[12402] = 10 / 428.7F;

            expectedRate[20220] = 1 / 682.0F;
            expectedRate[3827] = 1 / 818.4F;
            expectedRate[21387] = 1 / 750.2F;

            //TODO test entire med fire table?
            expectedRate[19835] = 1 / 30.0F;

            Assert.IsTrue(CheckAllRates(monster, expectedRate));
        }
    }
}
