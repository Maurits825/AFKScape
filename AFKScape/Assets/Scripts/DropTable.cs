using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DropTable
{
    private int numRolls;
    private List<Loot> lootItems = new List<Loot>();
    private struct Loot
    {
        public string item;
        public int amountMin;
        public int amountMax;

        public int chance;
        public int baseChance;
    }

    private Dictionary<string, int> rollTable()
    {
        Dictionary<string, int> retItems = new Dictionary<string, int>();

        for (int i = 0; i < numRolls; i++)
        {
            foreach (Loot loot in lootItems)
            {
                if (isLootDropped(loot.chance, loot.baseChance))
                {
                    int amount = getAmount(loot.amountMin, loot.amountMax);
                    retItems.Add(loot.item, amount);
                }
            }
        }

        return retItems;
    }

    private bool isLootDropped(int chance, int baseChance)
    {
        int num = Random.Range(1, baseChance);

        return (num < chance);
    }

    private int getAmount(int amountMin, int amountMax)
    {
        return Random.Range(amountMin, amountMax);
    }
}
