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

            List<(long, int)> items;
            items = generalDropTable.RollTable();

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
            long itemId;

            Dictionary<long, int> cluesInv = new Dictionary<long, int>() {
                { 2677, 0 },
                { 2801, 0 },
                { 2722, 0 },
                { 12073, 0 },
                { 23182, 0 }};

            for (int i = 0; i < iterations; i++)
            {
                (itemId, _) = clueDropTable.RollTable(fishLevel);
                if (itemId != -1)
                {
                    cluesInv[itemId]++;
                }
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
            long petId;
            for (int i = 0; i < iterations; i++)
            {
                (petId, _) = petDropTable.RollTable(fishLvl);
                if (petId != -1)
                {
                    petNum++;
                }
            }

            Assert.AreEqual(iterations, petNum);
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator TestSuiteWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}
