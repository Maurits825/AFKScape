using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class ItemTest
    {
        // A Test behaves as an ordinary method
        [Test]
        public void TestItemParsing()
        {
            Item item = new Item();
            TextAsset JSONFile = Resources.Load<TextAsset>(string.Concat("JSON/", "Item_test"));
            item = JsonUtility.FromJson<Item>(JSONFile.text);

            Assert.AreEqual("Abyssal whip", item.info.name);
        }
    }
}
