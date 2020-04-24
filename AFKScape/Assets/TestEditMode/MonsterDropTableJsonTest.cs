using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Linq;

namespace Tests
{
    public class MonsterDropTableJsonTest
    {
        [Test]
        public void ZulrahDropTableJsonTest()
        {
            Monster Zulrah = new Monster("Zulrah");
            int baseChance = Zulrah.monsterDropTableHandler.baseChance;
            int lastIndex = Zulrah.monsterDropTableHandler.indexMapping.Last();

            foreach (MonsterDropTable.BasicLoot loot in Zulrah.monsterDropTableHandler.basicLoots)
            {
                Assert.Greater(loot.id, 0);
                Assert.Greater(loot.amountMin, 0);
                Assert.Greater(loot.amountMax, 0);
            }

            foreach (GeneralDropTable generalDropTable in Zulrah.monsterDropTableHandler.generalDropTables)
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

            foreach (MonsterDropTable monsterDropTable in Zulrah.monsterDropTableHandler.monsterDropTables)
            {
                Assert.Greater(monsterDropTable.baseChance, 0);
                Assert.AreEqual(monsterDropTable.indexMapping.Count, monsterDropTable.basicLoots.Count);
                foreach (MonsterDropTable.BasicLoot item in monsterDropTable.basicLoots)
                {
                    Assert.Greater(item.id, 0);
                }
            }

            //TODO because zulrah
            Assert.AreEqual(baseChance, lastIndex + 1);
        }

        [Test]
        public void AllMonsterDropTableJsonTest()
        {
            //TODO use list later from bosses controller when its made
            List<Monster> monsters = new List<Monster>();
            Monster Vorkath = new Monster("Vorkath");
            monsters.Add(Vorkath);

            foreach (Monster monster in monsters)
            {
                int baseChance = monster.monsterDropTableHandler.baseChance;
                int lastIndex = monster.monsterDropTableHandler.indexMapping.Last();
                Assert.AreEqual(baseChance, lastIndex);

                int indexMapCount = monster.monsterDropTableHandler.indexMapping.Count;
                int lootAndTableCount = monster.monsterDropTableHandler.basicLoots.Count + monster.monsterDropTableHandler.monsterDropTables.Count;
                Assert.AreEqual(indexMapCount, lootAndTableCount);

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
                    Assert.Greater(monsterDropTable.baseChance, 0);
                    Assert.AreEqual(monsterDropTable.indexMapping.Count, monsterDropTable.basicLoots.Count);
                    foreach (MonsterDropTable.BasicLoot item in monsterDropTable.basicLoots)
                    {
                        Assert.Greater(item.id, 0);
                    }
                }
            }
        }

        [Test]
        public void RareDropTableTest()
        {
            MonsterDropTable monsterDropTable = JsonHandler.GetDropTable("rdt");
            int baseChance = monsterDropTable.baseChance;
            int lastIndex = monsterDropTable.indexMapping.Last();

            //TODO rounding issues...
            Assert.LessOrEqual(Mathf.Abs(lastIndex - baseChance), 150);

            Assert.Greater(monsterDropTable.baseChance, 0);

            foreach (MonsterDropTable.BasicLoot item in monsterDropTable.basicLoots)
            {
                Assert.Greater(item.id, 0);
            }
        }

        [Test]
        public void HerbSeedTableTest()
        {
            MonsterDropTable monsterDropTable = JsonHandler.GetDropTable("tree_herb_seed");
            int baseChance = monsterDropTable.baseChance;
            int lastIndex = monsterDropTable.indexMapping.Last();

            Assert.AreEqual(lastIndex, baseChance);
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
