using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill
{
    public string name;
    public int xp = 0;
    public int currentLevel = 1;
    public int boostedLevel = 1;

    public TextAsset trainingMethodsJSONFile;
    public List<TrainingMethod> trainingMethods = new List<TrainingMethod>();

    public Skill(string skillName)
    {
        name = skillName;
        trainingMethodsJSONFile = Resources.Load<TextAsset>(string.Concat("JSON/", name, "TrainingMethods"));        
    }

    public void populateTrainingMethods()
    {
        TrainingMethodList trainingMethodListJSON = JsonUtility.FromJson<TrainingMethodList>(trainingMethodsJSONFile.text);

        foreach (TrainingMethod trainingMethod in trainingMethodListJSON.trainingMethodList)
        {
            trainingMethods.Add(trainingMethod);
        }
    }
}
