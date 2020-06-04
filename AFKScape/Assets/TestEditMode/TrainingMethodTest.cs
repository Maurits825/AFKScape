using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Tests
{
    public class TrainingMethodTest
    {
        private Dictionary<string, Skill> skillsClasses = new Dictionary<string, Skill>();

        [SetUp]
        public void Setup()
        {
            Database.LoadExperienceTable();
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
        public void AllTrainingMethodTest()
        {
            foreach (Skill skill in skillsClasses.Values)
            {
                foreach (TrainingMethod method in skill.trainingMethods)
                {
                    Assert.IsNotEmpty(method.name);
                    Assert.Greater(method.xpPerResource, 0);
                    Assert.Greater(method.baseResourceRate, 0, method.name);

                    if (method.dropTables.Count > 0)
                    {
                        foreach (GeneralDropTable generalTable in method.dropTables.OfType<GeneralDropTable>())
                        {
                            Assert.IsNotEmpty(generalTable.name);
                            Assert.Greater(generalTable.numRolls, 0);
                            Assert.Greater(generalTable.lootItems.Count, 0);

                            foreach (DropTable.Loot loot in generalTable.lootItems)
                            {
                                Assert.Greater(loot.id, 0);
                                Assert.Greater(loot.amountMin, 0);
                                Assert.Greater(loot.amountMax, 0);
                                Assert.GreaterOrEqual(loot.amountMax, loot.amountMin);
                                Assert.Greater(loot.chance, 0);
                                Assert.Greater(loot.baseChance, 0);
                            }
                        }

                        foreach (ClueDropTable cluetable in method.dropTables.OfType<ClueDropTable>())
                        {
                            Assert.Greater(cluetable.lootItems[0].baseChance, 0);
                            Assert.AreEqual(1, cluetable.lootItems.Count);
                        }

                        foreach (PetDropTable petTable in method.dropTables.OfType<PetDropTable>())
                        {
                            Assert.Greater(petTable.lootItems[0].id, 0);
                            Assert.Greater(petTable.lootItems[0].baseChance, 0);
                            Assert.AreEqual(1, petTable.lootItems[0].amountMin);
                            Assert.AreEqual(1, petTable.lootItems[0].amountMax);

                            Assert.AreEqual(1, petTable.lootItems.Count);
                        }
                    }

                    Assert.Greater(method.requirements.levelRequirements.Count, 0);
                    Assert.AreEqual(skill.skillName, method.requirements.levelRequirements[0].skillName);

                    foreach(LevelRequirement lvlReq in method.requirements.levelRequirements)
                    {
                        Assert.IsNotEmpty(lvlReq.skillName);
                        Assert.Greater(lvlReq.levelReq, 0);
                    }
                }
            }
        }

        [Test]
        public void TrainingMethodConstructorTest()
        {
            string methodName = "myMethod";
            int resourceRate = 5;
            float xpPerResource = 2.5F;

            Requirements req = new Requirements();
            TrainingMethod trainingMethod = new TrainingMethod(methodName, resourceRate, req);
            trainingMethod.xpPerResource = xpPerResource;

            Assert.AreEqual(methodName, trainingMethod.name);
            Assert.AreEqual(resourceRate, trainingMethod.baseResourceRate);

            Assert.AreEqual(resourceRate * xpPerResource, trainingMethod.baseXpRate, 0.01);
        }

        [Test]
        public void LevelRequirementConstructorTest()
        {
            string name = "Agility";
            int lvl = 5;
            LevelRequirement levelRequirement = new LevelRequirement(name, lvl);

            Assert.AreEqual(name, levelRequirement.skillName);
            Assert.AreEqual(lvl, levelRequirement.levelReq);
        }
    }
}
