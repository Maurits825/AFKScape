using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct LevelRequirement
{
    public string SkillName;
    public int levelReq;

    public LevelRequirement(string name, int lvl)
    {
        SkillName = name;
        levelReq = lvl;
    }
}

[Serializable]
public struct Requirements
{
    public List<LevelRequirement> levelRequirements;
    public List<string> quest;
}

[Serializable]
public class TrainingMethod
{
    public string name;
    public int baseXpRate;

    public Requirements requirements;    

    public TrainingMethod(string methodName, int xpRate, Requirements req)
    {
        name = methodName;
        baseXpRate = xpRate;
        requirements = req;
    }
}

[Serializable]
public class TrainingMethodList
{
    public List<TrainingMethod> trainingMethodList;
}