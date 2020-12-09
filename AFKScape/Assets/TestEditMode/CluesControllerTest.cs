using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class CluesControllerTest
    {
        private CluesController cluesController;
        private Inventory inventory;
        private Bank bank;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            Database.LoadBosses();
            if (Database.items.Count != 0)
            {
                Database.items.Clear();
            }
            Database.LoadItems();
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            Database.bossesNames.Clear();
            Database.items.Clear();
        }

        [SetUp]
        public void SetUp()
        {
            EventManager.SetIntance(new EventManager());

            inventory = new Inventory();
            bank = new Bank();

            cluesController = new CluesController(inventory, bank);
        }

        [Test]
        public void ClueGameLoopTest()
        {
            Monster monster = cluesController.cluesClasses[Database.cluesNames[0]];

            int days = 1;
            int seconds = days * 86400;
            cluesController.OnClueSelected(Database.cluesNames[0]);
            cluesController.ClueGameLoop(monster, seconds / 10);
        }
    }
}
