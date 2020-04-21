﻿using System.Collections;
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

            Dictionary<long, int> dropTableDict;

            TrainingMethod trainingMethod = new TrainingMethod();
            trainingMethod.dropTables.Add(new GeneralDropTable());
            trainingMethod.dropTables[0].numRolls = 1;
            trainingMethod.dropTables[0].lootItems[0] = new DropTable.Loot(id);

            dropTableDict = DropTableManager.CreateDropTableDictionary(trainingMethod.dropTables);
            DropTableManager.RollResources(dropTableDict, trainingMethod, boostedLvl);
            Assert.AreEqual(1, dropTableDict[id]);

            trainingMethod.dropTables[0].numRolls = 2;
            dropTableDict = DropTableManager.CreateDropTableDictionary(trainingMethod.dropTables);
            DropTableManager.RollResources(dropTableDict, trainingMethod, boostedLvl);
            Assert.AreEqual(2, dropTableDict[id]);
        }
    }
}