using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster
{
    //stats from database
    int id;
    public string bossName;

    public int killCount;

    public MonsterDropTableHandler monsterDropTableHandler;

    public Monster(string name)
    {
        bossName = name;
        killCount = 0;        
    }

    public void GetDropTableHandler(string name)
    {
        monsterDropTableHandler = JsonHandler.GetMonsterDropTableHandler(name);
        
        if (monsterDropTableHandler.preMadeTables.Count != 0)
        {
            foreach (MonsterDropTableHandler.TableInfo tableInfo in monsterDropTableHandler.preMadeTables)
            {
                MonsterDropTable table = JsonHandler.GetMonsterDropTable(tableInfo.name);
                table.weight = tableInfo.weight;
                monsterDropTableHandler.monsterDropTables.Add(table);
            }
        }

        monsterDropTableHandler.SetTotalLootCount();
    }
}
