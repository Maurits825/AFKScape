using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TableDropTable
{
    public bool isChild;
    public int rolls;
    public int baseChance;
    public List<int> indexMapping;

    public List<DiceDropTable> diceDropTables; //TODO this here or item list
    public List<TableDropTable> tableDropTables;

    public TableDropTable(bool child)
    {
        isChild = child;
        diceDropTables = new List<DiceDropTable>();
        //tableDropTables = new List<TableDropTable>(); //TODO not always used
        indexMapping = new List<int>();
    }

    public void RollTable(Dictionary<long, int> dropTableDict)
    {
        if (isChild)
        {
            rolls = 1;
        }

        for (int r = 0; r < rolls; r++)
        {
            int index = UnityEngine.Random.Range(1, baseChance);
            int diceTableCount = diceDropTables.Count;

            for (int i = 0; i < indexMapping.Count; i++)
            {
                if (index > indexMapping[i])
                {
                    if (i < diceTableCount)
                    {
                        diceDropTables[i].RollTable(dropTableDict);
                    }
                    else
                    {
                        tableDropTables[i % diceTableCount].RollTable(dropTableDict);
                    }
                }
            }
        }
    }
}
