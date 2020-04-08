using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public struct LevelRequirement
{
    public string skillName;
    public int levelReq;

    public LevelRequirement(string name, int lvl)
    {
        skillName = name;
        levelReq = lvl;
    }
}

[Serializable]
public struct Requirements
{
    public List<LevelRequirement> levelRequirements;
    public List<string> quest;
    public List<string> items;
}

[Serializable]
public class TrainingMethod
{
    public string name;
    public int baseXpRate;
    public float xpPerResource;
    public float resourceRate { get { return baseXpRate / xpPerResource; } }
    public List<string> lootTable; //todo implemet a loot table class?

    public Requirements requirements;


    public TrainingMethod()
    {
        name = "";
        baseXpRate = 0;
        requirements = new Requirements();
    }
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