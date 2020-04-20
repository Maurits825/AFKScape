using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class DropTableTest
    {
        [Test]
        public void TestGeneralDropTable()
        {
            GeneralDropTable generalDropTable = new GeneralDropTable();
            long itemId = 315;
            DropTable.Loot loot = new DropTable.Loot(itemId);
            generalDropTable.lootItems[0] = loot;

            List<(long, int)> items = new List<(long, int)>();
            generalDropTable.RollTable(items);

            Assert.AreEqual(itemId, items[0].Item1);
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

            //TODO look at this ut
            Dictionary<long, int> cluesInv = new Dictionary<long, int>() {
                { 2677, 0 },
                { 2801, 0 },
                { 2722, 0 },
                { 12073, 0 },
                { 23182, 0 }};

            List<(long, int)> items = new List<(long, int)>();
            for (int i = 0; i < iterations; i++)
            {
                clueDropTable.RollTable(items, fishLevel);
            }

            Assert.IsTrue(true);
        }

        [Test]
        public void TestPetDropTable()
        {
            PetDropTable petDropTable = new PetDropTable();
            DropTable.Loot loot = new DropTable.Loot(0)
            {
                baseChance = 2501, //with lvl 100 = 1/1 chance
                id = 13320 // heron
            };
            petDropTable.lootItems[0] = loot;

            int fishLvl = 100;

            int iterations = 10;
            int petNum = 0;
            List<(long, int)> items = new List<(long, int)>();
            for (int i = 0; i < iterations; i++)
            {
                petDropTable.RollTable(items, fishLvl);
                if (items[i].Item1 != -1)
                {
                    petNum++;
                }
            }

            Assert.AreEqual(iterations, petNum);
        }
    }
}
