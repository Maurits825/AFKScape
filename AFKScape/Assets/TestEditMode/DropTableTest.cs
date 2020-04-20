using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class DropTableTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestGeneralDropTable()
        {
            GeneralDropTable generalDropTable = new GeneralDropTable();
            long itemId = 315;
            DropTable.Loot loot = new DropTable.Loot(itemId);
            generalDropTable.lootItems[0] = loot;

            List<DropTable> dropTables = new List<DropTable>
            {
                generalDropTable
            };

            Dictionary<long, int> dropTableDict = DropTableManager.CreateDropTableDictionary(dropTables);
            generalDropTable.RollTable(dropTableDict);

            Assert.AreEqual(1, dropTableDict[itemId]);
        }

        [Test]
        public void TestClueDropTable()
        {
            ClueDropTable clueDropTable = new ClueDropTable();

            DropTable.Loot loot = new DropTable.Loot(-1)
            {
                baseChance = 101 //with lvl 1 = 1/1 chance
            };
            clueDropTable.lootItems[0] = loot;

            int fishLevel = 1;
            int iterations = 1000;

            List<DropTable> dropTables = new List<DropTable>
            {
                clueDropTable
            };

            Dictionary<long, int> dropTableDict = DropTableManager.CreateDropTableDictionary(dropTables);
            for (int i = 0; i < iterations; i++)
            {
                //TODO this works because the fishing class has clue table
                clueDropTable.RollTable(dropTableDict, fishLevel);
            }

            Assert.IsTrue(true);
        }

        [Test]
        public void TestPetDropTable()
        {
            long heronId = 13320;
            PetDropTable petDropTable = new PetDropTable();
            DropTable.Loot loot = new DropTable.Loot(0)
            {
                baseChance = 2501, //with lvl 100 = 1/1 chance
                id = heronId
            };
            petDropTable.lootItems[0] = loot;

            int fishLvl = 100;
            int iterations = 10;

            List<DropTable> dropTables = new List<DropTable>
            {
                petDropTable
            };

            Dictionary<long, int> dropTableDict = DropTableManager.CreateDropTableDictionary(dropTables);
            for (int i = 0; i < iterations; i++)
            {
                petDropTable.RollTable(dropTableDict, fishLvl);
            }

            Assert.AreEqual(iterations, dropTableDict[heronId]);
        }
    }
}
