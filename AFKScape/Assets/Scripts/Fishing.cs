using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fishing : Skill
{
    public Fishing() : base("Fishing")
    {
        populateTrainingMethods();
    }

    public override void populateTrainingMethods()
    {
        Requirements req = new Requirements
        {
            level = 1,
            quest = ""
        };
        skillTrainingMethods.Add(new SkillTrainingMethod("Shrimp", 10, req));

        req = new Requirements
        {
            level = 30,
            quest = ""
        };
        skillTrainingMethods.Add(new SkillTrainingMethod("Salmon", 70, req));
    }
}
