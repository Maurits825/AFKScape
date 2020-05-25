using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

[Serializable]
public class ClueDropTable : DropTable
{
    public static readonly List<(int, int[])> clueChances = new List<(int, int[])>
        {
            (2677, new int[] { 4, 10 }),
            (2801, new int[] { 3, 10 }),
            (2722, new int[] { 2, 10 }),
            (12073, new int[] { 1, 10 }),
        };

    private static readonly int clueCount = 4;
    public static readonly long beginnerClueId = 23182;

    public ClueDropTable()
        : base("Clue")
    {
        tableType = DropTableType.Clue;
    }

    public ClueDropTable(DropTable dropTable) : base("Clue")
    {
        tableType = DropTableType.Clue;
        numRolls = dropTable.numRolls;
        lootItems = dropTable.lootItems;
    }

    public override void RollTable(Dictionary<long, BigInteger> dropTableDict, int skillLevel)
    {
        long clueId = GetClue(lootItems[0].chance, lootItems[0].baseChance, skillLevel);

        if (clueId != -1)
        {
            dropTableDict[clueId] += 1;
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
            return beginnerClueId; //beginner clue id
        }

        return clueId;
    }
}
