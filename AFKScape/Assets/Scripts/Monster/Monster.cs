using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster
{
    //stats from database
    int id;
    public string name;

    public int killCount;

    public MonsterDropTableHandler monsterDropTableHandler;

    public Monster(string mName)
    {
        name = mName;
        killCount = 0;
        monsterDropTableHandler = JsonHandler.GetMonster(name);
    }
}
