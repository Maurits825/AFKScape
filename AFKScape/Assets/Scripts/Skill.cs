using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill
{
    public string name;
    public int xp = 0;
    public int currentLevel = 1;
    public int boostedLevel = 1;
    
    public List<TrainingMethod> skillTrainingMethods = new List<TrainingMethod>();

    public abstract void populateTrainingMethods();

    public Skill(string skillName)
    {
        name = skillName;
    }
}
