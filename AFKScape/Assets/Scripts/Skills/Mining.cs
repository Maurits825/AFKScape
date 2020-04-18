using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mining : Skill
{
    public Mining() : base("Mining")
    {
        populateTrainingMethods(skillName);
    }
}
