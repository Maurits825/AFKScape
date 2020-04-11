using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class ItemTest
    {

        public static ItemList itemList;

        [Test]
        public void TestItemLoad()
        {
            Database.items.Clear();
            Database.LoadItems();
            long id = 15000;
            Assert.AreEqual(id, Database.items[id].id);
        }

        [Test]
        public void TestItemListParsing()
        {
            TextAsset JSONFile = Resources.Load<TextAsset>(string.Concat("JSON/Test/", "item"));
            itemList = JsonUtility.FromJson<ItemList>(JSONFile.text);
            Assert.NotNull(itemList.itemList[0].name);
        }
    }
}
