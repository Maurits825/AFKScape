﻿using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Linq;

namespace Tests
{
    public class MonsterDropTableJsonTest
    {
        private int GetWeightSum(MonsterDropTableHandler handler)
        {
            int weightSum = 0;
            foreach (MonsterDropTable.BasicLoot loot in handler.basicLoots)
            {
                weightSum += loot.weight;
            }

            foreach (MonsterDropTable table in handler.monsterDropTables)
            {
                weightSum += table.weight;
            }

            return weightSum;
        }

        private int GetWeightSum(List<MonsterDropTable.BasicLoot> basicLoots)
        {
            int weightSum = 0;
            foreach (MonsterDropTable.BasicLoot loot in basicLoots)
            {
                weightSum += loot.weight;
            }

            return weightSum;
        }

        [Test]
        public void AllMonsterDropTableJsonTest()
        {
            BossesController bossesController = new BossesController();
            bossesController.InitMonsterClasses();

            foreach (Monster monster in bossesController.bossesClasses.Values)
            {
                Debug.Log(monster.bossName);
                int baseChance = monster.monsterDropTableHandler.baseChance;
                int totalWeight = GetWeightSum(monster.monsterDropTableHandler);
                Assert.AreEqual(baseChance, totalWeight);

                int lootAndTableCount = monster.monsterDropTableHandler.basicLoots.Count + monster.monsterDropTableHandler.monsterDropTables.Count;
                Assert.AreEqual(lootAndTableCount, monster.monsterDropTableHandler.totalBasicLootCount);

                foreach (MonsterDropTable.BasicLoot loot in monster.monsterDropTableHandler.basicLoots)
                {
                    Assert.Greater(loot.id, 0);
                    Assert.Greater(loot.amountMin, 0);
                    Assert.Greater(loot.amountMax, 0);
                }

                foreach (GeneralDropTable generalDropTable in monster.monsterDropTableHandler.generalDropTables)
                {
                    Assert.Greater(generalDropTable.numRolls, 0);
                    foreach (DropTable.Loot item in generalDropTable.lootItems)
                    {
                        Assert.Greater(item.id, 0);
                        Assert.Greater(item.amountMin, 0);
                        Assert.Greater(item.amountMax, 0);
                        Assert.Greater(item.baseChance, 0);
                        Assert.Greater(item.chance, 0);
                    }
                }

                foreach (MonsterDropTable monsterDropTable in monster.monsterDropTableHandler.monsterDropTables)
                { 
                    baseChance = monsterDropTable.baseChance;
                    totalWeight = GetWeightSum(monsterDropTable.basicLoots);
                    Assert.AreEqual(baseChance, totalWeight);

                    Assert.Greater(monsterDropTable.baseChance, 0);
                    foreach (MonsterDropTable.BasicLoot item in monsterDropTable.basicLoots)
                    {
                        Assert.Greater(item.id, 0);
                        if (item.amountMax != 0)
                        {
                            Assert.Greater(item.amountMin, 0);
                            Assert.Greater(item.amountMax, 0);
                        }
                    }
                }
            }
        }

        [Test]
        public void RareDropTableTest()
        {
            MonsterDropTable monsterDropTable = JsonHandler.GetMonsterDropTable("rare_drop_table");
            int baseChance = monsterDropTable.baseChance;
            int totalWeight = GetWeightSum(monsterDropTable.basicLoots);

            Assert.AreEqual(totalWeight, totalWeight);
            Assert.Greater(monsterDropTable.baseChance, 0);

            foreach (MonsterDropTable.BasicLoot item in monsterDropTable.basicLoots)
            {
                Assert.Greater(item.id, 0);
            }
        }

        [Test]
        public void HerbSeedTableTest()
        {
            MonsterDropTable monsterDropTable = JsonHandler.GetMonsterDropTable("tree_herb_seed");
            int baseChance = monsterDropTable.baseChance;
            int totalWeight = GetWeightSum(monsterDropTable.basicLoots);

            Assert.AreEqual(totalWeight, totalWeight);
            Assert.Greater(monsterDropTable.baseChance, 0);

            foreach (MonsterDropTable.BasicLoot item in monsterDropTable.basicLoots)
            {
                Assert.Greater(item.id, 0);
                Assert.Greater(item.amountMin, 0);
                Assert.Greater(item.amountMax, 0);
            }
        }
    }
}
