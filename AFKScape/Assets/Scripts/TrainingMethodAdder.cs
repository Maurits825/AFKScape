using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingMethodAdder : MonoBehaviour
{
    private TextAsset trainingMethodsJSONFile;
    private TrainingMethodList trainingMethodListJSON;
    public List<TrainingMethod> trainingMethods = new List<TrainingMethod>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadJsonFile(string selectedSkillName)
    {
        trainingMethodsJSONFile = Resources.Load<TextAsset>(string.Concat("JSON/", selectedSkillName, "TrainingMethods"));
        trainingMethodListJSON = JsonUtility.FromJson<TrainingMethodList>(trainingMethodsJSONFile.text);

        trainingMethods.Clear();
        foreach (TrainingMethod trainingMethod in trainingMethodListJSON.trainingMethodList)
        {
            trainingMethods.Add(trainingMethod);
        }
    }

    public void saveJsonFile()
    {
        throw new NotImplementedException();
    }
}
