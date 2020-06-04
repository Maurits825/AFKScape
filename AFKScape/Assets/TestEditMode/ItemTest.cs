using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    public class ItemTest
    {

        public static ItemList itemList;

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
            TextAsset JSONFile = Resources.Load<TextAsset>("JSON/Items");
            itemList = JsonUtility.FromJson<ItemList>(JSONFile.text);
            Assert.NotNull(itemList.itemList[0].name);
        }
    }
}
