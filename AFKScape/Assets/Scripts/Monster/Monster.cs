using System.Collections.Generic;
using System.Numerics;

public class Monster
{
    //stats from database
    public int id;
    public string bossName;

    public int killCount;

    public MonsterDropTableHandler monsterDropTableHandler;

    protected Dictionary<long, BigInteger> dropTableDict;

    public Monster(string name)
    {
        bossName = name;
        killCount = 0;
    }

    public void GetDropTableHandler(string name)
    {
        monsterDropTableHandler = JsonHandler.GetMonsterDropTableHandler(name);

        if (monsterDropTableHandler.preMadeTables.Count != 0)
        {
            foreach (MonsterDropTableHandler.TableInfo tableInfo in monsterDropTableHandler.preMadeTables)
            {
                MonsterDropTable table = JsonHandler.GetMonsterDropTable(tableInfo.name);
                table.weight = tableInfo.weight;
                monsterDropTableHandler.monsterDropTables.Add(table);
            }
        }

        monsterDropTableHandler.SetTotalLootCount();
    }

    public Dictionary<long, BigInteger> CreateDropTableDictionary(List<MonsterDropTable> extraTables)
    {
        Dictionary<long, BigInteger> dropTable = new Dictionary<long, BigInteger>();

        foreach (MonsterDropTable.BasicLoot basicLoot in monsterDropTableHandler.basicLoots)
        {
            dropTable[basicLoot.id] = 0;
        }

        foreach (MonsterDropTable table in monsterDropTableHandler.monsterDropTables)
        {
            foreach (MonsterDropTable.BasicLoot basicLoot in table.basicLoots)
            {
                dropTable[basicLoot.id] = 0;
            }
        }

        if (extraTables != null && extraTables.Count == 0)
        {
            foreach (MonsterDropTable table in extraTables)
            {
                foreach (MonsterDropTable.BasicLoot basicLoot in table.basicLoots)
                {
                    dropTable[basicLoot.id] = 0;
                }
            }
        }

        foreach (GeneralDropTable table in monsterDropTableHandler.generalDropTables)
        {
            foreach (DropTable.Loot loot in table.lootItems)
            {
                dropTable[loot.id] = 0;
            }
        }

        return dropTable;
    }

    public Dictionary<long, BigInteger> GetDropTableDict()
    {
        return dropTableDict;
    }

    public virtual void KillBoss(Dictionary<long, BigInteger> dropTableDict)
    {
        monsterDropTableHandler.Roll(dropTableDict);
    }
}
