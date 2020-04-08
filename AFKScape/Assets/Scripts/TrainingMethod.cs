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
    private float resourceRate { get { return baseXpRate / xpPerResource; } }
    public List<DropTable> dropTables;

    public Requirements requirements;


    public TrainingMethod()
    {
        name = "";
        baseXpRate = 0;
        xpPerResource = 0;
        dropTables = new List<DropTable>() { new DropTable("General") };
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