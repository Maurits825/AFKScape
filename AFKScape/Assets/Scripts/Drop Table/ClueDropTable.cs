using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ClueDropTable : DropTable
{
    private static List<(int, int[])> clueChances = new List<(int, int[])>();
    private static int clueCount;

    public ClueDropTable() : base("Clue")
    {
        tableType = DropTableType.Clue;
    }

    public ClueDropTable(DropTable dropTable) : base("Clue")
    {
        tableType = DropTableType.Clue;
        numRolls = dropTable.numRolls;
        lootItems = dropTable.lootItems;
    }

    static ClueDropTable()
    {
        clueChances.Add((2677, new int[] { 4, 10 }));
        clueChances.Add((2801, new int[] { 3, 10 }));
        clueChances.Add((2722, new int[] { 2, 10 }));
        clueChances.Add((12073, new int[] { 1, 10 }));
        clueCount = clueChances.Count;
    }

    public override void RollTable(List<(long, int)> itemList, int skillLevel)
    {
        long clueId = GetClue(lootItems[0].chance, lootItems[0].baseChance, skillLevel);

        if (clueId != -1)
        {
            itemList.Add((clueId, 1));
        }
    }

    private long GetClue(int chance, int baseChance, int skillLevel)
    {
        long clueId = -1;

        int accuracyGain = 100;
        int actualChance = chance * accuracyGain;
        int actualBaseChance = Mathf.FloorToInt((baseChance / (100 + skillLevel)) * accuracyGain);

        if (IsLootDropped(actualChance, actualBaseChance))
        {
            for (int i = 0; i < clueCount; i++)
            {
                if (IsLootDropped(clueChances[i].Item2[0], clueChances[i].Item2[1]))
                {
                    return clueChances[i].Item1;
                }
            }
        }

        //roll beginner clue seperatly
        if (IsLootDropped(1, 1000))
        {
            return 23182; //beginner clue id
        }

        return clueId;
    }
}
