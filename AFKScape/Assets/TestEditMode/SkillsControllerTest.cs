using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class SkillsControllerTest
    {
        private SkillsController skillsController;
        private Inventory inventory;
        private Bank bank;

        [SetUp]
        public void SetUp()
        {
            EventManager.setIntance(new EventManager());
            Database.LoadExperienceTable();
            skillsController = new SkillsController();
            inventory = new Inventory();
            bank = new Bank();

            skillsController.Initialize(inventory, bank);
        }

        // A Test behaves as an ordinary method
        [Test]
        public void MainGameLoopTest()
        {
            Fishing fishing = new Fishing();

            skillsController.SkillButtonClicked("Fishing");
            skillsController.SetTrainingMethod(0);

            int days = 5;
            int seconds = days * 86400;
            skillsController.MainGameLoop(fishing.trainingMethods[0], fishing, seconds);

            Assert.AreEqual(true, true);
        }

        [Test]
        public void GetLevelTest()
        {
            Database.LoadExperienceTable();
            Assert.AreEqual(63, SkillsController.GetLevel(407014));
            Assert.AreEqual(64, SkillsController.GetLevel(407015));
            Assert.AreEqual(64, SkillsController.GetLevel(407016));

            Assert.AreEqual(99, SkillsController.GetLevel(13034431));
            Assert.AreEqual(125, SkillsController.GetLevel(171077457));
            Assert.AreEqual(126, SkillsController.GetLevel(188884740));
            Assert.AreEqual(126, SkillsController.GetLevel(200000000));
        }

        [Test]
        public void CheckRequirementTest()
        {
            Mining mining = new Mining();
            Cooking cooking = new Cooking();

            Assert.AreEqual(false, skillsController.CheckRequirement(mining.trainingMethods[0]));
            inventory.AddItem(1267,1);
            Assert.AreEqual(true, inventory.Contains(1267));
            Assert.AreEqual(true, skillsController.CheckRequirement(mining.trainingMethods[0]));
            Assert.AreEqual(true, skillsController.CheckRequirement(cooking.trainingMethods[0]));
            Assert.AreEqual(false, skillsController.CheckRequirement(cooking.trainingMethods[1]));
        }

    }
}
