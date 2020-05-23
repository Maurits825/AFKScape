using System;
using System.Collections.Generic;
using System.Numerics;

[Serializable]
public class MonsterDropTable
{
    public string name;
    public int weight;

    public int baseChance;
    public List<BasicLoot> basicLoots;

    [Serializable]
    public struct BasicLoot
    {
        [ItemId]
        public long id;

        public int weight;
        public int amountMin;
        public int amountMax;
    }

    public MonsterDropTable()
    {
        basicLoots = new List<BasicLoot>();
    }

    public int GetAmount(int amountMin, int amountMax)
    {
        return UnityEngine.Random.Range(amountMin, amountMax + 1);
    }

    public void AddLoot(Dictionary<long, BigInteger> dropTableDict, BasicLoot loot)
    {
        int amount = GetAmount(loot.amountMin, loot.amountMax);
        dropTableDict[loot.id] += amount;
    }
    public virtual bool Roll(Dictionary<long, BigInteger> dropTableDict)
    {
        int index = UnityEngine.Random.Range(1, baseChance + 1);
        int weightSum = 0;

        for (int i = 0; i < basicLoots.Count; i++)
        {
            weightSum += basicLoots[i].weight;
            if (index <= weightSum)
            {
                AddLoot(dropTableDict, basicLoots[i]);
                return true;
            }
        }

        return false;
    }
}
