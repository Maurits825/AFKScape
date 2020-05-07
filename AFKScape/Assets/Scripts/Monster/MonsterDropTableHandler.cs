using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

[Serializable]
public class MonsterDropTableHandler : MonsterDropTable
{
    public int rolls;

    public List<GeneralDropTable> generalDropTables; //will include 100% drops and tertiary, they are all rolled individually
    public List<MonsterDropTable> monsterDropTables;

    public List<TableInfo> preMadeTables;

    [Serializable]
    public struct TableInfo
    {
        public string name;
        public int weight;

        public TableInfo(string n, int w)
        {
            name = n;
            weight = w;
        }
    }

    public int totalBasicLootCount;

    public MonsterDropTableHandler()
    {
        generalDropTables = new List<GeneralDropTable>();
        monsterDropTables = new List<MonsterDropTable>();
    }

    public override bool Roll(Dictionary<long, BigInteger> dropTableDict)
    {
        RollGeneral(dropTableDict);

        for (int r = 0; r < rolls; r++)
        {
            RollBasic(dropTableDict);
        }

        return true;
    }

    public void RollGeneral(Dictionary<long, BigInteger> dropTableDict)
    {
        //roll general items individually, roll is handled in the table
        for (int i = 0; i < generalDropTables.Count; i++)
        {
            generalDropTables[i].RollTable(dropTableDict);
        }
    }

    public bool RollBasic(Dictionary<long, BigInteger> dropTableDict)
    {
        int index = UnityEngine.Random.Range(1, baseChance + 1);
        int basicLootCount = basicLoots.Count;

        int weightSum = 0;
        for (int i = 0; i < totalBasicLootCount; i++)
        {
            if (i < basicLootCount)
            {
                weightSum += basicLoots[i].weight;
                if (index <= weightSum)
                {
                    AddLoot(dropTableDict, basicLoots[i]);
                    return true;
                }
            }
            else
            {
                weightSum += monsterDropTables[i % basicLootCount].weight;
                if (index <= weightSum)
                {
                    monsterDropTables[i % basicLootCount].Roll(dropTableDict);
                    return true;
                }
            }
        }

        return false;
    }

    public void SetTotalLootCount()
    {
        totalBasicLootCount = basicLoots.Count + monsterDropTables.Count;
    }
}
