using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    public class ItemTest
    {
        public static ItemList itemList;

        private long whipId = 4151;
        private long dragonBootsId = 11840;

        [OneTimeTearDown]
        public void Cleanup()
        {
            Database.items.Clear();
        }

        [Test]
        public void ItemLoadTest()
        {
            Database.items.Clear();
            Database.LoadItems();
            long id = 15000;
            Assert.AreEqual(id, Database.items[id].id);
        }

        [Test]
        public void ItemListParsingTest()
        {
            Database.LoadItems();

            //Assert.AreEqual(Equipment.EquipmentSlot.weapon, Database.items[whipId].equipment.slot);
            Assert.AreEqual(Equipment.EquipmentSlot.feet, Database.items[dragonBootsId].equipment.slot);
        }
    }
}
