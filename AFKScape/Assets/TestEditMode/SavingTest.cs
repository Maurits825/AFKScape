using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;

namespace Tests
{
    public class SavingTest
    {
        MainController mainController;

        [SetUp]

        public void Setup()
        {
            Database.LoadExperienceTable();
            mainController = new MainController(); //TODO fix this warning
            mainController.InitSkillClasses();
        }
        
        [Test]
        public void SaveTest()
        {
            mainController.skillsClasses["Cooking"].xpFloat = 2500;
            SaveHandler saveHandler = new SaveHandler();
            saveHandler.SaveGame();
            mainController.skillsClasses["Cooking"].xpFloat = 10000;
            mainController.skillsClasses["Fishing"].xpFloat = 83;
            saveHandler.LoadGame();
            //Assert.AreEqual(2500, mainController.skillsClasses["Cooking"].xp);
            //Assert.AreNotEqual(83, mainController.skillsClasses["Fishing"].xp);
        }
    }
}
