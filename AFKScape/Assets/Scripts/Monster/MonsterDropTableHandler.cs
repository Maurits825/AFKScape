using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MonsterDropTableHandler : MonsterDropTable
{
    public int rolls;
    public GeneralDropTable generalDropTable; //will include 100% drops and tertiary, they are all rolled individually
    public List<MonsterDropTable> monsterDropTables;

    public MonsterDropTableHandler()
    {
        generalDropTable = new GeneralDropTable();
        monsterDropTables = new List<MonsterDropTable>();
    }

    public override void RollTable(Dictionary<long, int> dropTableDict)
    {
        for (int r = 0; r < rolls; r++)
        {
            //roll general items individually, rolls is handled here, the rolls in general table is left as default=1
            generalDropTable.RollTable(dropTableDict);

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
}
