﻿using System.Collections.Generic;
using UnityEngine;

public class TrainingMethodAdder : MonoBehaviour
{
    public List<TrainingMethod> trainingMethods = new List<TrainingMethod>();

    public void LoadJsonFile(string selectedSkillName)
    {
        trainingMethods = JsonHandler.GetTrainingMethods(selectedSkillName);
    }

    public void SaveJsonFile(string selectedSkillName)
    {
        JsonHandler.SaveJsonFile(trainingMethods, selectedSkillName);
    }
}
