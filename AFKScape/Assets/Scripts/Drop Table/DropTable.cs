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
        public string item;
        public int amountMin;
        public int amountMax;

        public int chance;
        public int baseChance;

        public Loot(string itemName)
        {
            item = itemName; //TODO item should be item name maybe beacuse it a string, not intance of Item class
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

    public virtual List<(string, int)> RollTable()
    {
        return new List<(string, int)>() { (null, 0) };
    }

    public virtual (string, int) RollTable(int skillLevel) //is this the way to do it?
    {
        return (null, 0);
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
