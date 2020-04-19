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
            Assert.AreEqual(64, MainController.getLevel(407015));
            Assert.AreEqual(99, MainController.getLevel(13034431));
            Assert.AreEqual(125, MainController.getLevel(171077457));
            Assert.AreEqual(126, MainController.getLevel(188884740));
            Assert.AreEqual(126, MainController.getLevel(200000000));
        }

        [Test]
        public void LevelRequirementTest()
        {
            
        }

    }
}
