using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PetDropTable : DropTable
{
    public PetDropTable() : base("Pet")
    {
    }

    public override (string, int) RollTable(int skillLevel)
    {
        string retItem = null;
        int retAmount = 0;

        if (isPet(lootItems[0].chance, lootItems[0].baseChance, skillLevel))
        {
            retItem = lootItems[0].item;
            retAmount = 1;
        }

        return (retItem, retAmount);
    }

    private bool isPet(int chance, int baseChance, int skillLevel)
    {
        string retClue = null;

        int accuracyGain = 100;
        int actualChance = chance * accuracyGain;
        int actualBaseChance = Mathf.FloorToInt((baseChance - (skillLevel * 25)) * accuracyGain);

        return IsLootDropped(actualChance, actualBaseChance);
    }
}
