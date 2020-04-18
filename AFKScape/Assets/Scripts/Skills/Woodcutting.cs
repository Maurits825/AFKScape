using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Woodcutting : Skill
{
    public Woodcutting() : base("Woodcutting")
    {
        populateTrainingMethods(skillName);
    }
}
