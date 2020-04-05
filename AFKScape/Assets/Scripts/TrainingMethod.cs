using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Requirements
{
    public int level;
    public string quest;
}

public class TrainingMethod
{
    public Requirements requirements;

    public string name;
    public int baseXpRate;

    public TrainingMethod(string methodName, int xpRate, Requirements req)
    {
        name = methodName;
        baseXpRate = xpRate;
        requirements = req;
    }

}
