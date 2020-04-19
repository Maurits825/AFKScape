using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
public class Requirements
{
    public List<LevelRequirement> levelRequirements;
    public List<int> questIDs;
    public List<long> itemIDs;
    public List<long> generalSkillItems; //Require at least of these items to start skill

    public Requirements()
    {
        levelRequirements = new List<LevelRequirement>();
        questIDs = new List<int>();
        itemIDs = new List<long>();
        generalSkillItems = new List<long>();
    }
}

[Serializable]
public struct Consumables
{
    public long itemId;
    public int amount;
}

[Serializable]
public class TrainingMethod
{
    public string name;
    public float baseXpRate { get { return xpPerResource * baseResourceRate; } }
    public float xpPerResource;
    public float baseResourceRate;

    public List<Consumables> consumables;

    public List<DropTable> dropTables;

    public Requirements requirements;

    public TrainingMethod()
    {
        name = "";
        baseResourceRate = 0;
        xpPerResource = 0;
        consumables = new List<Consumables>();
        dropTables = new List<DropTable>();
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