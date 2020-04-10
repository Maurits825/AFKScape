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
    public float baseXpRate { get { return xpPerResource * baseResourceRate; } }
    public float xpPerResource;
    public float baseResourceRate;

    public List<DropTable> dropTables;

    public Requirements requirements;

    public TrainingMethod()
    {
        name = "";
        baseResourceRate = 0;
        xpPerResource = 0;
        dropTables = new List<DropTable>() { new GeneralDropTable() };
        requirements = new Requirements();
    }
    public TrainingMethod(string methodName, int resourceRate, Requirements req)
    {
        name = methodName;
        baseResourceRate = resourceRate;
        requirements = req;
    }
}

[Serializable]
public class TrainingMethodList
{
    public List<TrainingMethod> trainingMethodList;
}