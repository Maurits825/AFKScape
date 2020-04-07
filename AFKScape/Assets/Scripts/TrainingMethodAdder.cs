using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

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
        trainingMethodsJSONFile = Resources.Load<TextAsset>(string.Concat("JSON/TrainingMethods/", selectedSkillName));
        trainingMethodListJSON = JsonUtility.FromJson<TrainingMethodList>(trainingMethodsJSONFile.text);

        trainingMethods.Clear();
        if (trainingMethodListJSON.trainingMethodList.Count > 0)
        {
            foreach (TrainingMethod trainingMethod in trainingMethodListJSON.trainingMethodList)
            {
                trainingMethods.Add(trainingMethod);
            }
        }
    }

    public void saveJsonFile(string selectedSkillName)
    {
        TrainingMethodList trainingMethodList = new TrainingMethodList();
        trainingMethodList.trainingMethodList = trainingMethods;

        string JSONString = JsonUtility.ToJson(trainingMethodList);

        //TODO better way for path?
        File.WriteAllText(string.Concat(Application.dataPath, "/Resources/JSON/TrainingMethods/", selectedSkillName, ".json"), JSONString);
        AssetDatabase.Refresh();
    }
}
