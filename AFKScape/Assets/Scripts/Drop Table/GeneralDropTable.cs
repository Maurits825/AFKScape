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

    public override void RollTable(List<(long, int)> itemList)
    {
        for (int r = 0; r < numRolls; r++)
        {
            for (int i = 0; i < lootItems.Count; i++)
            {
                Loot loot = lootItems[i];
                if (IsLootDropped(loot.chance, loot.baseChance)) //TODO this is wrong
                {
                    int amount = GetAmount(loot.amountMin, loot.amountMax);
                    itemList.Add((loot.id, amount));
                }
            }
        }
    }
}
