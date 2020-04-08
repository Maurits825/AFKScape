using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DropTable
{
    public string name;
    public int numRolls;
    public List<Loot> lootItems;

    [Serializable]
    public struct Loot
    {
        public string item;
        public int amountMin;
        public int amountMax;

        public int chance;
        public int baseChance;

        public Loot(string itemName)
        {
            item = itemName;
            amountMin = 1;
            amountMax = 1;
            chance = 1;
            baseChance = 1;
        }
    }

    public DropTable(string n)
    {
        name = n;
        numRolls = 1;
        lootItems = new List<Loot>() { new Loot("") };
    }

    public virtual Dictionary<string, int> RollTable()
    {
        Dictionary<string, int> retItems = new Dictionary<string, int>();

        for (int i = 0; i < numRolls; i++)
        {
            foreach (Loot loot in lootItems)
            {
                if (IsLootDropped(loot.chance, loot.baseChance))
                {
                    int amount = GetAmount(loot.amountMin, loot.amountMax);
                    retItems.Add(loot.item, amount);
                }
            }
        }

        return retItems;
    }

    public virtual Dictionary<string, int> RollTable(int skillLevel)
    {
        return RollTable();
    }

    private int GetAmount(int amountMin, int amountMax)
    {
        return UnityEngine.Random.Range(amountMin, amountMax);
    }

    public bool IsLootDropped(int chance, int baseChance)
    {
        int num = UnityEngine.Random.Range(1, baseChance);
        return (num < chance);
    }
}
