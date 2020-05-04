using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

[Serializable]
public class MonsterDropTable
{
    public string name;
    public int baseChance;
    public List<int> indexMapping;
    public List<BasicLoot> basicLoots;

    [Serializable]
    public struct BasicLoot
    {
        [ItemId]
        public long id;

        public int amountMin;
        public int amountMax;
    }

    public MonsterDropTable()
    {
        indexMapping = new List<int>();
        basicLoots = new List<BasicLoot>();
    }

    public int GetAmount(int amountMin, int amountMax)
    {
        return UnityEngine.Random.Range(amountMin, amountMax);
    }

    public void AddLoot(Dictionary<long, BigInteger> dropTableDict, BasicLoot loot)
    {
        int amount = GetAmount(loot.amountMin, loot.amountMax);
        dropTableDict[loot.id] += amount;
    }
    public virtual void RollTable(Dictionary<long, BigInteger> dropTableDict)
    {
        int index = UnityEngine.Random.Range(1, baseChance);
        for (int i = 0; i < indexMapping.Count; i++)
        {
            if (index < indexMapping[i])
            {
                AddLoot(dropTableDict, basicLoots[i]);
                break;
            }
        }
    }
}
