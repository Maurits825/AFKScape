﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClueDropTable : DropTable
{
    private static Dictionary<string, int[]> clueChances = new Dictionary<string, int[]>();

    public ClueDropTable() : base("Clue")
    {
        clueChances.Add("easy", new int[]{ 4, 10});
        clueChances.Add("medium", new int[] { 3, 10 });
        clueChances.Add("hard", new int[] { 2, 10 });
        clueChances.Add("elite", new int[] { 1, 10 });
        clueChances.Add("beginner", new int[] { 1, 1000 });
    }

    public override Dictionary<string, int> RollTable(int skillLevel)
    {
        Dictionary<string, int> retItems = new Dictionary<string, int>();

        string clueName = GetClue(lootItems[0].chance, lootItems[0].baseChance, skillLevel);

        if (clueName != null)
        {
            retItems.Add(clueName, 1);
        }

        return retItems;
    }

    private string GetClue(int chance, int baseChance, int skillLevel)
    {
        string retClue = null;

        int accuracyGain = 100;
        int actualChance = chance * accuracyGain;
        int actualBaseChance = Mathf.FloorToInt((baseChance / (100 + skillLevel)) * accuracyGain);

        if (IsLootDropped(actualBaseChance, actualBaseChance))
        {
            foreach (KeyValuePair<string, int[]> clue in clueChances)
            {
                if (IsLootDropped(clue.Value[0], clue.Value[1]))
                {
                    return clue.Key;
                }
            }
        }

        return retClue;
    }
}
