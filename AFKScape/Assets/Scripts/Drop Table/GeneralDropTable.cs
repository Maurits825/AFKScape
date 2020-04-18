using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class GeneralDropTable : DropTable
{
    public GeneralDropTable() : base("General")
    {
        tableType = DropTableType.General;
    }

    public GeneralDropTable(DropTable dropTable) : base("General")
    {
        tableType = DropTableType.General;
        numRolls = dropTable.numRolls;
        lootItems = dropTable.lootItems;
    }

    public override List<(long, int)> RollTable()
    {
        List<(long, int)> retList = new List<(long, int)>();

        long itemId = 0;
        int amount = 0;

        for (int i = 0; i < numRolls; i++)
        {
            foreach (Loot loot in lootItems)
            {
                if (IsLootDropped(loot.chance, loot.baseChance)) //TODO this is wrong
                {
                    itemId = loot.id;
                    amount = GetAmount(loot.amountMin, loot.amountMax);

                    retList.Add((itemId, amount));
                }
            }
        }

        return retList;
    }
}
