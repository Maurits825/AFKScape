using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vorkath : Monster
{
    public Vorkath() : base("Vorkath")
    {
        GetDropTableHandler(bossName);
    }
}
