using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Numerics;

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

    public override void RollTable(Dictionary<long, BigInteger> dropTableDict)
    {
        //TODO leave this as is for now in terms of perf
        for (int r = 0; r < numRolls; r++)
        {
            for (int i = 0; i < lootItems.Count; i++)
            {
                Loot loot = lootItems[i];
                //TODO if chance=base=1 no need for rolling
                //TODO look at this, min=max=1 no need to call getamount
                if (IsLootDropped(loot.chance, loot.baseChance))
                {
                    int amount = GetAmount(loot.amountMin, loot.amountMax); 
                    dropTableDict[loot.id] += amount;
                }
            }
        }
    }
}
