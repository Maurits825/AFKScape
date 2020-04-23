using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster
{
    //stats from database
    int id;
    public string name;
    //other stuff

    public MonsterDropTableHandler monsterDropTableHandler;

    public Monster(string mName)
    {
        name = mName;
        monsterDropTableHandler = JsonHandler.GetMonster(name);
    }

}
