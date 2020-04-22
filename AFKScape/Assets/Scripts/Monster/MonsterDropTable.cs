using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MonsterDropTable
{
    public int rolls;
    public int baseChance;
    public List<int> indexMapping;

    public List<DiceDropTable> diceDropTable;

    public MonsterDropTable()
    {
        diceDropTable = new List<DiceDropTable>();
        indexMapping = new List<int>();
        baseChance = 1;
    }

    public void RollTable(Dictionary<long, int> dropTableDict)
    {
        for (int r = 0; r < rolls; r++)
        {
            int index = UnityEngine.Random.Range(1, baseChance);

            for (int i = 0; i < indexMapping.Count; i++)
            {
                if (index > indexMapping[i])
                {
                    diceDropTable[i].RollTable(dropTableDict);
                }
            }
        }
    }
}
