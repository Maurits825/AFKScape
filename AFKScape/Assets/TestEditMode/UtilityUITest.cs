using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class UtilityUITest
    {
        // A Test behaves as an ordinary method
        [Test]
        public void FormatNumberTest()
        {
            string text;
            Color color;

            (text, color) = UtilityUI.FormatNumber(55000);
            Assert.AreEqual("55000", text);
            Assert.AreEqual(Color.yellow, color);

            (text, color) = UtilityUI.FormatNumber(100_000);
            Assert.AreEqual("100K", text);
            Assert.AreEqual(Color.white, color);

            (text, color) = UtilityUI.FormatNumber(110_000);
            Assert.AreEqual("110K", text);
            Assert.AreEqual(Color.white, color);

            (text, color) = UtilityUI.FormatNumber(7_860_450);
            Assert.AreEqual("7860K", text);
            Assert.AreEqual(Color.white, color);

            (text, color) = UtilityUI.FormatNumber(10_000_000);
            Assert.AreEqual("10M", text);
            Assert.AreEqual(Color.green, color);

            (text, color) = UtilityUI.FormatNumber(11_450_000);
            Assert.AreEqual("11M", text);
            Assert.AreEqual(Color.green, color);

            (text, color) = UtilityUI.FormatNumber(980_450_000);
            Assert.AreEqual("980M", text);
            Assert.AreEqual(Color.green, color);

            (text, color) = UtilityUI.FormatNumber(1_000_000_000);
            Assert.AreEqual("1B", text);
            Assert.AreEqual(Color.blue, color);

            (text, color) = UtilityUI.FormatNumber(2_980_000_000);
            Assert.AreEqual("2B", text);
            Assert.AreEqual(Color.blue, color);

            (text, color) = UtilityUI.FormatNumber(45_000_000_000_000);
            Assert.AreEqual("45T", text);
            Assert.AreEqual(Color.red, color);
        }
    }
}
