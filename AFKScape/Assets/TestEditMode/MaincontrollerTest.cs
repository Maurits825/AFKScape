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

            EventManager.setIntance(new EventManager()); //TODO fix warning
            MainController mainController = new MainController(); //TODO fix this warning
            mainController.InitStatic();
            mainController.MainGameLoop(fishing.trainingMethods[0], fishing, 0.5F);

            Assert.AreEqual(true, true);
        }

        [Test]
        public void LevelsTest()
        {
            Database.LoadLevels();
            Assert.AreEqual(MainController.getLevel(407015), 64);
            Assert.AreEqual(MainController.getLevel(13034431), 99);
            Assert.AreEqual(MainController.getLevel(100000000), 99);
        }

    }
}
