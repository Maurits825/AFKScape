using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;

namespace Tests
{
    public class SkillsTest
    {
        // A Test behaves as an ordinary method
        [Test]
        public void SkillsTestSimplePasses()
        {

        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator SkillsTestWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}
