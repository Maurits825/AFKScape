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
            string item = "Shrimp";
            DropTable.Loot loot = new DropTable.Loot(item);
            generalDropTable.lootItems[0] = loot;

            List<(string, int)> items;
            items = generalDropTable.RollTable();

            Assert.AreEqual(item, items[0].Item1);
        }

        [Test]
        public void TestClueDropTable()
        {
            ClueDropTable clueDropTable = new ClueDropTable();

            DropTable.Loot loot = new DropTable.Loot("")
            {
                baseChance = 101 //with lvl 1 = 1/1 chance
            };
            clueDropTable.lootItems[0] = loot;

            int fishLevel = 1;

            int iterations = 1000;
            string item;

            Dictionary<string, int> cluesInv = new Dictionary<string, int>() {
                { "easy", 0 },
                { "medium", 0 },
                { "hard", 0 },
                { "elite", 0 },
                { "beginner", 0 }};

            for (int i = 0; i < iterations; i++)
            {
                (item, _) = clueDropTable.RollTable(fishLevel);
                if (!string.IsNullOrEmpty(item))
                {
                    cluesInv[item]++;
                }
            }

            Assert.IsTrue(true);
        }

        [Test]
        public void TestPetDropTable()
        {
            PetDropTable petDropTable = new PetDropTable();
            DropTable.Loot loot = new DropTable.Loot("")
            {
                baseChance = 2501, //with lvl 100 = 1/1 chance
                item = "Heron"
            };
            petDropTable.lootItems[0] = loot;

            int fishLvl = 100;

            int iterations = 10;
            int petNum = 0;
            string pet;
            for (int i = 0; i < iterations; i++)
            {
                (pet, _) = petDropTable.RollTable(fishLvl);
                if (!string.IsNullOrEmpty(pet))
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
