using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor;
using UnityEngine;

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

    public override void RollTable(Dictionary<long, BigInteger> dropTableDict)
    {
        //roll general items individually, roll is handled in the table
        for (int i = 0; i < generalDropTables.Count; i++)
        {
            generalDropTables[i].RollTable(dropTableDict);
        }

        for (int r = 0; r < rolls; r++)
        {
            int index = UnityEngine.Random.Range(1, baseChance);
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
                        break;
                    }
                }
                else
                {
                    weightSum += monsterDropTables[i % basicLootCount].weight;
                    if (index <= weightSum)
                    {
                        monsterDropTables[i % basicLootCount].RollTable(dropTableDict);
                        break;
                    }
                }
            }
        }
    }

    public Dictionary<long, BigInteger> CreateDropTableDictionary()
    {
        Dictionary<long, BigInteger> dropTableDict = new Dictionary<long, BigInteger>();

        foreach (BasicLoot basicLoot in basicLoots)
        {
            dropTableDict[basicLoot.id] = 0;
        }

        foreach (MonsterDropTable table in monsterDropTables)
        {
            foreach (BasicLoot basicLoot in table.basicLoots)
            {
                dropTableDict[basicLoot.id] = 0;
            }
        }

        foreach (GeneralDropTable table in generalDropTables)
        {
            foreach (DropTable.Loot loot in table.lootItems)
            {
                dropTableDict[loot.id] = 0;
            }
        }

        return dropTableDict;
    }

    public void SetTotalLootCount()
    {
        totalBasicLootCount = basicLoots.Count + monsterDropTables.Count;
    }
}
