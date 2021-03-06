﻿using System.Collections.Generic;

public abstract class Skill
{
    public string skillName;

    public int xp
    {
        get { return (int)xpFloat; }
    }

    public float xpFloat = 0;
    public int currentLevel = 1;
    public int boostedLevel = 1;
    public int xpNextLvl = 0;

    public List<TrainingMethod> trainingMethods = new List<TrainingMethod>();

    protected Skill(string name)
    {
        skillName = name;
        xpNextLvl = Database.experienceTable[1]; //TODO could hardcode 83 here if loading sync issues
    }

    public void PopulateTrainingMethods(string skillName)
    {
        trainingMethods = JsonHandler.GetTrainingMethods(skillName);
    }

    //included here the actualXprate? this is calculated using basexprate and lvl, items, perks...
    //can provided a default way to calc this that can be overridden
    public virtual float GetResourceRate(float baseResourceRate)
    {
        float lvlBoost = 0.1F;
        return baseResourceRate + (baseResourceRate * lvlBoost * boostedLevel);

        //TODO -- something like this for each skill
        //if equipedItems.handSlot == dragon pickaxe then PickaxeBoost = 1.2
        //if perks.twoHanded then pickAxeBoost*2
    }
}
