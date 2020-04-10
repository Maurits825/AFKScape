using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class ItemTest
    {
        [Test]
        public void TestItemListParsing()
        {
            ItemList itemList = new ItemList();
            TextAsset JSONFile = Resources.Load<TextAsset>(string.Concat("JSON/Test/", "item"));
            itemList = JsonUtility.FromJson<ItemList>(JSONFile.text);

            Assert.AreEqual("Abyssal whip", itemList.itemList[0].info.name);
        }
    }
}
