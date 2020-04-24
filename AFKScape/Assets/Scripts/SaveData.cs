using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    public Dictionary<string, float> skills = new Dictionary<string, float>();


    public void SetSkills(Skill skill)
    {
        skills.Add(skill.skillName, skill.xpFloat);
    }

    public Dictionary<string, float> GetSkills()
    {
        return skills;
    }
}
