using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class DropTableManagerTest
    {
        [Test]
        public void RollResourcesTest()
        {
            int boostedLvl = 50;
            long id = 500;

            List<(long, int)> items = new List<(long, int)>();

            TrainingMethod trainingMethod = new TrainingMethod();
            trainingMethod.dropTables.Add(new GeneralDropTable());
            trainingMethod.dropTables[0].numRolls = 1;
            trainingMethod.dropTables[0].lootItems[0] = new DropTable.Loot(id);

            DropTableManager.RollResources(items, trainingMethod, boostedLvl);
            Assert.AreEqual(id, items[0].Item1);
            Assert.AreEqual(1, items[0].Item2);
            Assert.AreEqual(1, items.Count);

            items.Clear();
            trainingMethod.dropTables[0].numRolls = 2;
            DropTableManager.RollResources(items, trainingMethod, boostedLvl);
            Assert.AreEqual(id, items[0].Item1);
            Assert.AreEqual(1, items[0].Item2);
            Assert.AreEqual(2, items.Count);
        }
    }
}
