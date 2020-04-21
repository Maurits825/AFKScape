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
            Database.LoadExperienceTable();
            Fishing fishing = new Fishing();

            EventManager.setIntance(new EventManager()); //TODO fix warning
            MainController mainController = new MainController(); //TODO fix this warning
            mainController.InitStatic();
            mainController.InitSkillClasses();
            mainController.OnSkillSelected("Fishing");
            mainController.SetTrainingMethod(0);
            mainController.MainGameLoop(fishing.trainingMethods[0], fishing, 86400);

            Assert.AreEqual(true, true);
        }

        [Test]
        public void LevelsTest()
        {
            Database.LoadExperienceTable();
            Assert.AreEqual(63, MainController.GetLevel(407014));
            Assert.AreEqual(64, MainController.GetLevel(407015));
            Assert.AreEqual(64, MainController.GetLevel(407016));

            Assert.AreEqual(99, MainController.GetLevel(13034431));
            Assert.AreEqual(125, MainController.GetLevel(171077457));
            Assert.AreEqual(126, MainController.GetLevel(188884740));
            Assert.AreEqual(126, MainController.GetLevel(200000000));
        }

        [Test]
        public void RequirementTest()
        {
            Mining mining = new Mining();
            Cooking cooking = new Cooking();
            MainController mainController = new MainController(); //TODO fix this warning
            mainController.InitSkillClasses();
            Assert.AreEqual(false, mainController.CheckRequirement(mining.trainingMethods[0]));
            MainController.inventory.AddItem(1267,1);
            Assert.AreEqual(true,mainController.CheckRequirement(mining.trainingMethods[0]));
            Assert.AreEqual(true, mainController.CheckRequirement(cooking.trainingMethods[0]));
            Assert.AreEqual(false, mainController.CheckRequirement(cooking.trainingMethods[1]));
        }

    }
}
