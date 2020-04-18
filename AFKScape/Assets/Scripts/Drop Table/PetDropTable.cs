using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PetDropTable : DropTable
{
    public PetDropTable() : base("Pet")
    {
        tableType = DropTableType.Pet;
    }

    public override (long, int) RollTable(int skillLevel)
    {
        long itemId = -1;
        int amount = 0;

        if (isPet(lootItems[0].chance, lootItems[0].baseChance, skillLevel))
        {
            itemId = lootItems[0].id;
            amount = 1;
        }

        return (itemId, amount);
    }

    private bool isPet(int chance, int baseChance, int skillLevel)
    {
        int accuracyGain = 100;
        int actualChance = chance * accuracyGain;
        int actualBaseChance = Mathf.FloorToInt((baseChance - (skillLevel * 25)) * accuracyGain);

        return IsLootDropped(actualChance, actualBaseChance);
    }
}
