using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class Clue
{
    public string clueName;

    private Dictionary<long, BigInteger> dropTableDict;

    public Dictionary<long, BigInteger> GetDropTableDict()
    {
        return dropTableDict;
    }
}
