using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;


namespace Tests
{
    public class StorageTest
    {
        private Storage storage;
        private long heronId = 13320;
        private long dragonMedId = 1149;

        [SetUp]
        public void Setup()
        {
            storage = new Storage();
            storage.totalSlots = 10;
            storage.AddItem(heronId, 1);
            storage.AddItem(heronId, 1);
            storage.AddItem(dragonMedId, 8);
        }

        [Test]
        public void AddItemTest()
        {
            Assert.AreEqual(true, storage.Contains(heronId));
            Assert.AreEqual(true, storage.Contains(dragonMedId));
        }

        [Test]
        public void RemoveItemTest()
        {
            storage.RemoveItem(heronId, 1);
            Assert.AreEqual(true, storage.Contains(heronId));
            storage.RemoveItem(heronId, 1);
            Assert.AreEqual(false, storage.Contains(heronId));
            storage.RemoveAll();
            Assert.AreEqual(false, storage.Contains(dragonMedId));
        }
    }
}
