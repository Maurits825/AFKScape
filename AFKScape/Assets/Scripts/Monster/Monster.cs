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
        /*
        if (monsterDropTableHandler.preMadeTables.Count != 0)
        {
            foreach ((string,int) tableInfo in monsterDropTableHandler.preMadeTables)
            {
                MonsterDropTable table = JsonHandler.GetMonsterDropTable(tableInfo.Item1);
                table.weight = tableInfo.Item2;
                monsterDropTableHandler.monsterDropTables.Add(table);
            }
        }*/

        monsterDropTableHandler.SetTotalLootCount();
    }
}
