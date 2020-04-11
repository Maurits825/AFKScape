using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class MaincontrollerTest
    {
        // A Test behaves as an ordinary method
        [Test]
        public void MainGameLoopTest()
        {
            Fishing fishing = new Fishing();

            MainController mainController = new MainController(); //TODO fix this warning
            mainController.InitStatic();
            mainController.MainGameLoop(fishing.trainingMethods[0], fishing, 60.0F);

            Assert.AreEqual(true, true);
        }

    }
}
