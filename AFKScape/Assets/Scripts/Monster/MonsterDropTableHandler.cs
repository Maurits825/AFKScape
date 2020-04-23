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
        for (int r = 0; r < rolls; r++)
        {
            //roll general items individually
            for (int i = 0; i < generalDropTables.Count; i++)
            {
                generalDropTables[i].RollTable(dropTableDict);
            }
            
            int index = UnityEngine.Random.Range(1, baseChance);
            int tableCount = monsterDropTables.Count;

            for (int i = 0; i < indexMapping.Count; i++)
            {
                if (index > indexMapping[i])
                {
                    if (i < tableCount)
                    {
                        AddLoot(dropTableDict, basicLoots[i]);
                    }
                    else
                    {
                        monsterDropTables[i % tableCount].RollTable(dropTableDict);
                    }
                }
            }
        }
    }

    public Dictionary<long, int> CreateDropTableDictionary()
    {
        Dictionary<long, int> dropTableDict = new Dictionary<long, int>();
        foreach (MonsterDropTable table in monsterDropTables)
        {
            foreach (BasicLoot basicLoot in table.basicLoots)
            {
                dropTableDict.Add(basicLoot.id, 0);
            }
        }

        foreach (GeneralDropTable table in generalDropTables)
        {

        }
        return dropTableDict;
    }
}
