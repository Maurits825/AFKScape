using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fishing : Skill
{
    public Fishing() : base("Fishing")
    {
        populateTrainingMethods(skillName);
    }
}
