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
        public void AllMonsterDropTableJsonTest()
        {
            //TODO use list later when its made and loop
            Monster Zulrah = new Monster("Zulrah");
            int baseChance = Zulrah.monsterDropTableHandler.baseChance;
            int lastIndex = Zulrah.monsterDropTableHandler.indexMapping.Last();

            //basic structure of loop, add to the foreach loop through bosses later
            foreach (MonsterDropTable.BasicLoot loot in Zulrah.monsterDropTableHandler.basicLoots)
            {
                Assert.Greater(loot.id, 0);
                Assert.Greater(loot.amountMin, 0);
                if (loot.amountMax == 0)
                {
                    int a = 5;
                }
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

            //TODO because zulrah
            Assert.AreEqual(baseChance, lastIndex + 1);
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
    }
}
