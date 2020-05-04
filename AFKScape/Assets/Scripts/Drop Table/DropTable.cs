using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Numerics;

[Serializable]
public class DropTable
{
    public string name;
    public DropTableType tableType;
    public int numRolls;
    public List<Loot> lootItems;

    public enum DropTableType
    {
        General,
        Clue,
        Pet,
    }

    [Serializable]
    public struct Loot
    {
        [ItemId]
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

    public virtual void RollTable(Dictionary<long, BigInteger> dropTableDict)
    {
    }

    public virtual void RollTable(Dictionary<long, BigInteger> dropTableDict, int skillLevel) //is this the way to do it?
    {
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
