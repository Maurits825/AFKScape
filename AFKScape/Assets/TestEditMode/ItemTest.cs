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
            TextAsset JSONFile = Resources.Load<TextAsset>(string.Concat("JSON/Test/", "item"));
            itemList = JsonUtility.FromJson<ItemList>(JSONFile.text);
            Assert.AreEqual(true, true);
        }

        [Test]
        public void TestItemListParsing()
        {
            Assert.NotNull(itemList.itemList[0].name);
        }
    }
}
