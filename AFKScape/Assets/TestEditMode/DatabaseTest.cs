using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class DatabaseTest
    {
        [Test]
        public void LoadAllTest()
        {
            Database.LoadAll();

            Assert.IsNotNull(Database.skillNames[0]);
            Assert.IsNotNull(Database.bossesNames[0]);
            Assert.IsNotNull(Database.items[0]);
            //Assert.IsNotNull(Database.quest[0]);
            Assert.IsNotNull(Database.experienceTable[0]);
            Assert.IsNotNull(Database.sprites[0]);
        }
    }
}
