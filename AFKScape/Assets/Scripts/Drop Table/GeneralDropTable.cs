using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class GeneralDropTable : DropTable
{
    public GeneralDropTable() : base("General")
    {

    }
    public override List<(string, int)> RollTable()
    {
        List<(string, int)> retList = new List<(string, int)>();

        string itemName = null;
        int amount = 0;

        for (int i = 0; i < numRolls; i++)
        {
            foreach (Loot loot in lootItems)
            {
                if (IsLootDropped(loot.chance, loot.baseChance)) //TODO this is wrong
                {
                    itemName = loot.item;
                    amount = GetAmount(loot.amountMin, loot.amountMax);

                    retList.Add((itemName, amount));
                }
            }
        }

        return retList;
    }
}
