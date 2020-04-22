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

    public override void RollTable(Dictionary<long, int> dropTableDict)
    {
        //TODO leave this as is for now in terms of perf
        //TODO this will change when adding the actual dice sim
        for (int r = 0; r < numRolls; r++)
        {
            for (int i = 0; i < lootItems.Count; i++)
            {
                Loot loot = lootItems[i];
                if (IsLootDropped(loot.chance, loot.baseChance)) //TODO this is wrong, well it works for tertiary type drops
                {
                    int amount = GetAmount(loot.amountMin, loot.amountMax); //TODO look at this, min=max=1 no need to call
                    dropTableDict[loot.id] += amount;
                }
            }
        }
    }
}
