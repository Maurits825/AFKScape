using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill
{
    public string name;
    public int xp { get { return (int)xpFloat; } }
    public float xpFloat = 0;
    public int currentLevel = 1;
    public int boostedLevel = 1;

    public TextAsset trainingMethodsJSONFile; //TODO is this neededhere?
    public List<TrainingMethod> trainingMethods = new List<TrainingMethod>();

    public Skill(string skillName)
    {
        name = skillName;
        trainingMethodsJSONFile = Resources.Load<TextAsset>(string.Concat("JSON/TrainingMethods/", name));        
    }

    public void populateTrainingMethods()
    {
        TrainingMethodList trainingMethodListJSON = JsonUtility.FromJson<TrainingMethodList>(trainingMethodsJSONFile.text);

        foreach (TrainingMethod trainingMethod in trainingMethodListJSON.trainingMethodList)
        {
            trainingMethods.Add(trainingMethod);
        }
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
