using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

[Serializable]
public class PetDropTable : DropTable
{
    public PetDropTable()
        : base("Pet")
    {
        tableType = DropTableType.Pet;
    }

    public PetDropTable(DropTable dropTable)
        : base("Pet")
    {
        tableType = DropTableType.Pet;
        numRolls = dropTable.numRolls;
        lootItems = dropTable.lootItems;
    }

    public override void RollTable(Dictionary<long, BigInteger> dropTableDict, int skillLevel)
    {
        if (IsPet(lootItems[0].chance, lootItems[0].baseChance, skillLevel))
        {
            dropTableDict[lootItems[0].id] += 1;
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
