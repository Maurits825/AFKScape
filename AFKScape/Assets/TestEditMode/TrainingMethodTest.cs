using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class TrainingMethodTest
    {
        private Dictionary<string, Skill> skillsClasses = new Dictionary<string, Skill>();

        [SetUp]
        public void Setup()
        {
            skillsClasses.Clear();
            skillsClasses.Add("Agility", new Agility());
            skillsClasses.Add("Construction", new Construction());
            skillsClasses.Add("Cooking", new Cooking());
            skillsClasses.Add("Crafting", new Crafting());
            skillsClasses.Add("Farming", new Farming());
            skillsClasses.Add("Firemaking", new Firemaking());
            skillsClasses.Add("Fishing", new Fishing());
            skillsClasses.Add("Fletching", new Fletching());
            skillsClasses.Add("Herblore", new Herblore());
            skillsClasses.Add("Hunter", new Hunter());
            skillsClasses.Add("Mining", new Mining());
            skillsClasses.Add("Prayer", new Prayer());
            skillsClasses.Add("Runecraft", new Runecraft());
            skillsClasses.Add("Smithing", new Smithing());
            skillsClasses.Add("Thieving", new Thieving());
            skillsClasses.Add("Woodcutting", new Woodcutting());
        }

        [Test]
        public void TestAllTrainingMethod()
        {
            foreach (Skill skill in skillsClasses.Values)
            {
                foreach (TrainingMethod method in skill.trainingMethods)
                {
                    Assert.IsNotEmpty(method.name);
                    Assert.Greater(method.xpPerResource, 0);
                    Assert.Greater(method.baseResourceRate, 0);

                    if (method.dropTables.Count > 0)
                    {
                        foreach (GeneralDropTable generalTable in method.dropTables.OfType<GeneralDropTable>())
                        {
                            Assert.IsNotEmpty(generalTable.name);
                            Assert.Greater(generalTable.numRolls, 0);
                            Assert.Greater(generalTable.lootItems.Count, 0);
                        }

                        foreach (ClueDropTable cluetable in method.dropTables.OfType<ClueDropTable>())
                        {
                            Assert.Greater(cluetable.lootItems[0].baseChance, 0);
                        }

                        foreach (PetDropTable petTable in method.dropTables.OfType<PetDropTable>())
                        {
                            Assert.Greater(petTable.lootItems[0].id, 0);
                            Assert.Greater(petTable.lootItems[0].baseChance, 0);
                        }
                    }

                    Assert.Greater(method.requirements.levelRequirements.Count, 0);
                    Assert.AreEqual(skill.skillName, method.requirements.levelRequirements[0].skillName);
                    Assert.Greater(method.requirements.levelRequirements[0].levelReq, 0);
                }
            }
        }
    }
}
