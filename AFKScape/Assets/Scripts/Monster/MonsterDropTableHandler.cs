using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MonsterDropTableHandler : MonsterDropTable
{
    public int rolls;
    public List<GeneralDropTable> generalDropTables; //will include 100% drops and tertiary, they are all rolled individually
    public List<MonsterDropTable> monsterDropTables;

    public MonsterDropTableHandler()
    {
        generalDropTables = new List<GeneralDropTable>();
        monsterDropTables = new List<MonsterDropTable>();
    }

    public override void RollTable(Dictionary<long, int> dropTableDict)
    {
        //roll general items individually, roll is handled in the table
        for (int i = 0; i < generalDropTables.Count; i++)
        {
            generalDropTables[i].RollTable(dropTableDict);
        }

        for (int r = 0; r < rolls; r++)
        {
            int index = UnityEngine.Random.Range(1, baseChance);
            int basicLootCount = basicLoots.Count;

            for (int i = 0; i < indexMapping.Count; i++)
            {
                if (index < indexMapping[i])
                {
                    if (i < basicLootCount)
                    {
                        AddLoot(dropTableDict, basicLoots[i]);
                        break;
                    }
                    else
                    {
                        monsterDropTables[i % basicLootCount].RollTable(dropTableDict);
                    }
                }
            }
        }
    }

    public Dictionary<long, int> CreateDropTableDictionary()
    {
        Dictionary<long, int> dropTableDict = new Dictionary<long, int>();

        foreach (BasicLoot basicLoot in basicLoots)
        {
            dropTableDict[basicLoot.id] = 0;
        }

        foreach (MonsterDropTable table in monsterDropTables)
        {
            foreach (BasicLoot basicLoot in table.basicLoots)
            {
                dropTableDict[basicLoot.id] = 0;
            }
        }

        foreach (GeneralDropTable table in generalDropTables)
        {
            foreach (DropTable.Loot loot in table.lootItems)
            {
                dropTableDict[loot.id] = 0;
            }
        }

        return dropTableDict;
    }
}
