using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prayer : Skill
{
    public Prayer() : base("Prayer")
    {
        populateTrainingMethods(skillName);
    }
}
