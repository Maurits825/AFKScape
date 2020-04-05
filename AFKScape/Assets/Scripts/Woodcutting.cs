using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Woodcutting : Skill
{
    public Woodcutting() : base("Woodcutting")
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
        skillTrainingMethods.Add(new TrainingMethod("Tree", 25, req));

        req = new Requirements
        {
            level = 15,
            quest = ""
        };
        skillTrainingMethods.Add(new TrainingMethod("Oak", 38, req));
    }
}
