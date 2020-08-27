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

        private const int Iterations = 1_000_000;
        private const float PercentThreshold = 20.0F;
        private const int TotalAttempts = 5;

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
            for (int attempt = 0; attempt < TotalAttempts; attempt++)
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
                        ", Sim: " + simRate +
                        ", Expected/Sim drops: " + Mathf.RoundToInt(expectedRate[id] * Iterations).ToString() + ", " + dropTableDict[id].ToString());

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
        public void ClueEasyDropRateTest()
        {
            Monster monster = new Monster(Database.cluesNames[1]);
            monster.Initialize();

            Dictionary<long, float> expectedRate = new Dictionary<long, float>();
            expectedRate[10366] = 1 / 360.0F;
            expectedRate[20166] = 1 / 1404.0F;
            expectedRate[20205] = 1 / 2808.0F;
            expectedRate[20211] = 1 / 5616.0F;
            expectedRate[20199] = 1 / 14040.0F;

            expectedRate[1165] = 1 / 36.0F;
            expectedRate[847] = 1 / 40.0F;
            expectedRate[10280] = 1 / 360.0F;

            //firelighter
            expectedRate[7329] = 4 / 180.0F;
            expectedRate[10326] = 4 / 180.0F;

            //teleports
            expectedRate[12402] = 10 / 452.6F;
            expectedRate[12403] = 10 / 452.6F;

            //blessing
            expectedRate[20220] = 1 / 2160.0F;
            expectedRate[20232] = 1 / 2160.0F;

            //pages
            expectedRate[3827] = 1 / 864.0F;
            expectedRate[3835] = 1 / 864.0F;
            expectedRate[12620] = 1 / 864.0F;

            //master scroll book
            expectedRate[21387] = 1 / 792.0F;

            expectedRate[19835] = 1 / 50.0F;
            Assert.IsTrue(CheckAllRates(monster, expectedRate));
        }

        [Test]
        public void ClueMediumDropRateTest()
        {
            Monster monster = new Monster(Database.cluesNames[2]);
            monster.Initialize();

            Dictionary<long, float> expectedRate = new Dictionary<long, float>();
            expectedRate[2577] = 1 / 1133.0F;
            expectedRate[23389] = 1 / 1133.0F;
            expectedRate[20266] = 1 / 2266.0F;
            expectedRate[10416] = 1 / 2266.0F;

            expectedRate[1161] = 1 / 34.1F;
            expectedRate[556] = 75 / 34.1F;
            expectedRate[1271] = 1 / 34.1F;

            expectedRate[855] = 1 / 37.9F;
            expectedRate[1731] = 1 / 37.9F;

            expectedRate[10364] = 1 / 341.0F;
            expectedRate[10282] = 1 / 341.0F;

            expectedRate[20275] = 1 / 341.0F;

            //firelighter
            expectedRate[7329] = 7 / 189.4F;
            expectedRate[10326] = 7 / 189.4F;            

            //teleports
            expectedRate[12402] = 10 / 428.7F;
            expectedRate[12403] = 10 / 428.7F;

            //blessing
            expectedRate[20220] = 1 / 682.0F;
            expectedRate[20232] = 1 / 682.0F;

            //pages
            expectedRate[3827] = 1 / 818.4F;
            expectedRate[3835] = 1 / 818.4F;
            expectedRate[12620] = 1 / 818.4F;

            //master scroll book
            expectedRate[21387] = 1 / 750.2F;

            expectedRate[19835] = 1 / 30.0F;

            Assert.IsTrue(CheckAllRates(monster, expectedRate));
        }

        [Test]
        public void ClueEliteDropRateTest()
        {
            Monster monster = new Monster(Database.cluesNames[4]);
            monster.Initialize();

            Dictionary<long, float> expectedRate = new Dictionary<long, float>();
            
            expectedRate[12538] = 1 / 1250.0F;
            expectedRate[19970] = 1 / 12500.0F;

            expectedRate[20005] = 1 / 28750.0F; //TODO test
            expectedRate[3486] = 1 / 63250.0F;
            expectedRate[12424] = 1 / 488750.0F;

            expectedRate[1127] = 1 / 32.3F;
            expectedRate[8778] = 70 / 32.3F;
            expectedRate[985] = 1 / 64.6F;
            expectedRate[5255] = 1 / 96.9F;

            
            //TODO all good down here expect master clue book
            //firelighter
            expectedRate[7329] = 12 / 161.5F;
            expectedRate[10326] = 12 / 161.5F;

            //teleports
            expectedRate[12402] = 10 / 203.0F;
            expectedRate[12403] = 10 / 203.0F;

            //blessing
            expectedRate[20220] = 1 / 645.8F;
            expectedRate[20232] = 1 / 645.8F;

            //pages
            expectedRate[3827] = 1 / 775.0F;
            expectedRate[3835] = 1 / 775.0F;
            expectedRate[12620] = 1 / 775.0F;

            //master scroll book
            expectedRate[21387] = 1 / 355.2F;

            expectedRate[19835] = 1 / 5.0F;

            Assert.IsTrue(CheckAllRates(monster, expectedRate));
        }
    }
}
