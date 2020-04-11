using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class DropTable
{
    public string name;
    public int numRolls;
    public List<Loot> lootItems;

    [Serializable]
    public struct Loot
    {
        public long id;
        public int amountMin;
        public int amountMax;

        public int chance;
        public int baseChance;

        public Loot(long idNum)
        {
            id = idNum;
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
        lootItems = new List<Loot>() { new Loot(0) };
    }

    public virtual List<(long, int)> RollTable()
    {
        return new List<(long, int)>() { (0, 0) };
    }

    public virtual (long, int) RollTable(int skillLevel) //is this the way to do it?
    {
        return (0, 0);
    }

    public int GetAmount(int amountMin, int amountMax)
    {
        return UnityEngine.Random.Range(amountMin, amountMax);
    }

    public bool IsLootDropped(int chance, int baseChance)
    {
        int num = UnityEngine.Random.Range(1, baseChance);
        return (num <= chance);
    }
}
