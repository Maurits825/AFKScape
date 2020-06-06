using NUnit.Framework;
using System.Numerics;

namespace Tests
{
    public class StorageTest
    {
        private Storage storage;
        private long heronId = 13320;
        private long dragonMedId = 1149;
        private int totalSlots = 5;

        [SetUp]
        public void Setup()
        {
            storage = new Storage();
            storage.totalSlots = totalSlots;
        }

        [Test]
        public void AddItemTest()
        {
            storage.AddItem(heronId, 1);
            storage.AddItem(dragonMedId, 8);
            Assert.IsTrue(storage.Contains(heronId));
            Assert.IsTrue(storage.Contains(dragonMedId));            
        }

        [Test]
        public void AddZeroItemTest()
        {
            storage.AddItem(dragonMedId, 0);
            Assert.IsFalse(storage.Contains(dragonMedId));
        }

        [Test]
        public void RemoveItemTest()
        {
            storage.AddItem(heronId, 1);
            storage.AddItem(heronId, 1);
            storage.AddItem(dragonMedId, 8);

            storage.RemoveItem(heronId, 1);
            Assert.IsTrue(storage.Contains(heronId));

            storage.RemoveItem(heronId, 1);
            Assert.IsFalse(storage.Contains(heronId));

            storage.RemoveAll();
            Assert.IsFalse(storage.Contains(dragonMedId));
        }

        [Test]
        public void FullStorageTest()
        {
            bool itemAdded;
            for (int i = 0; i < 5; i++)
            {
                itemAdded = storage.AddItem(i, 1);
                Assert.IsTrue(itemAdded);
            }

            itemAdded = storage.AddItem(dragonMedId, 1);
            Assert.IsFalse(itemAdded);
            Assert.IsFalse(storage.Contains(dragonMedId));
        }

        [Test]
        public void RemoveMoreTest()
        {
            storage.AddItem(dragonMedId, 2);
            BigInteger amountRemoved = storage.RemoveItem(dragonMedId, 5);
            Assert.AreEqual((BigInteger)2, amountRemoved);
            Assert.IsFalse(storage.Contains(dragonMedId));
        }

        [Test]
        public void RemoveNonExistingTest()
        {
            storage.AddItem(dragonMedId, 2);
            BigInteger amountRemoved = storage.RemoveItem(heronId, 5);
            Assert.AreEqual((BigInteger)0, amountRemoved);
        }
    }
}
