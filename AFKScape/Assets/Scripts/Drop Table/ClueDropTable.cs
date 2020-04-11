using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ClueDropTable : DropTable
{
    private static Dictionary<int, int[]> clueChances = new Dictionary<int, int[]>();

    public ClueDropTable() : base("Clue")
    {
        
    }

    static ClueDropTable()
    {
        clueChances.Add(2677, new int[] { 4, 10 });
        clueChances.Add(2801, new int[] { 3, 10 });
        clueChances.Add(2722, new int[] { 2, 10 });
        clueChances.Add(12073, new int[] { 1, 10 });
        clueChances.Add(23182, new int[] { 1, 1000 });
    }

    public override (long, int) RollTable(int skillLevel)
    {
        long itemId = -1;
        int amount = 0;

        long clueId = GetClue(lootItems[0].chance, lootItems[0].baseChance, skillLevel);

        if (clueId != -1)
        {
            itemId = clueId;
            amount = 1;
        }

        return (itemId, amount);
    }

    private long GetClue(int chance, int baseChance, int skillLevel)
    {
        long clueId = -1;

        int accuracyGain = 100;
        int actualChance = chance * accuracyGain;
        int actualBaseChance = Mathf.FloorToInt((baseChance / (100 + skillLevel)) * accuracyGain);

        if (IsLootDropped(actualChance, actualBaseChance))
        {
            foreach (KeyValuePair<int, int[]> clue in clueChances)
            {
                if (IsLootDropped(clue.Value[0], clue.Value[1]))
                {
                    return clue.Key;
                }
            }
        }

        return clueId;
    }
}
