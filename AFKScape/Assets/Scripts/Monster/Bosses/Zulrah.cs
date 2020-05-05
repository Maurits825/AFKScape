using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zulrah : Monster
{
    public Zulrah() : base("Zulrah")
    {
        GetDropTableHandler(bossName);
    }
}
