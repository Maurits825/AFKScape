﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill
{
    public string skillName;
    public int xp { get { return (int)xpFloat; } }
    public float xpFloat = 0;
    public int currentLevel = 1;
    public int boostedLevel = 1;

    public List<TrainingMethod> trainingMethods = new List<TrainingMethod>();

    public Skill(string name)
    {
        skillName = name;      
    }

    public void populateTrainingMethods(string skillName)
    {
        trainingMethods = JsonHandler.getTrainingMethods(skillName);
    }

    //included here the actualXprate? this is calculated using basexprate and lvl, items, perks...
    //can provided a default way to calc this that can be overridden
    public virtual float GetResourceRate(float baseResourceRate)
    {
        float lvlBoost = 0.1F;
        return baseResourceRate + (baseResourceRate * lvlBoost * boostedLevel);

        //TODO -- something like this for each skill
        //if equipedItems.handSlot == dragon pickaxe {PickaxeBoost = 1.2}
        //if perks.twoHanded {pickAxeBoost*2}
    }
}
