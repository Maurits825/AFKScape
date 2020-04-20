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

    public PetDropTable(DropTable dropTable) : base("Pet")
    {
        tableType = DropTableType.Pet;
        numRolls = dropTable.numRolls;
        lootItems = dropTable.lootItems;
    }


    public override void RollTable(List<(long, int)> itemList, int skillLevel)
    {
        if (IsPet(lootItems[0].chance, lootItems[0].baseChance, skillLevel))
        {
            itemList.Add((lootItems[0].id, 1));
        }
    }

    private bool IsPet(int chance, int baseChance, int skillLevel)
    {
        int accuracyGain = 100;
        int actualChance = chance * accuracyGain;
        int actualBaseChance = Mathf.FloorToInt((baseChance - (skillLevel * 25)) * accuracyGain);

        return IsLootDropped(actualChance, actualBaseChance);
    }
}
