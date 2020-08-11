using NUnit.Framework;

namespace Tests
{
    public class DatabaseTest
    {
        [OneTimeTearDown]
        public void Cleanup()
        {
            Database.bossesNames.Clear();
            Database.items.Clear();
            Database.experienceTable.Clear();
            Database.sprites.Clear();
        }

        [Test]
        public void LoadAllTest()
        {
            Database.LoadAll();

            Assert.IsNotNull(Database.skillNames[0]);
            Assert.IsNotNull(Database.bossesNames[0]);
            Assert.IsNotNull(Database.items[0]);
            //Assert.IsNotNull(Database.quest[0]);
            Assert.IsNotNull(Database.experienceTable[0]);
            Assert.IsNotNull(Database.sprites[0]);
        }
    }
}
