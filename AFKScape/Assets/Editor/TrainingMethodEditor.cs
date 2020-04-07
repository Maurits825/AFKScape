using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TrainingMethodAdder))]
public class trainingMethodEditor : Editor
{
    SerializedProperty trainingMethod;
    private string selectedSkillName;
    public static bool[] isTrainMethodSelected = new bool[2] { false, false }; //TODO init this from list size
    int selected = 0;

    void OnEnable()
    {
        // Fetch the objects from the GameObject script to display in the inspector
        trainingMethod = serializedObject.FindProperty("trainingMethods");
    }

    public override void OnInspectorGUI()
    {
        TrainingMethodAdder myTarget = (TrainingMethodAdder)target;

        EditorGUILayout.HelpBox("Load a JSON file from a skill", MessageType.Info);

        selected = EditorGUILayout.Popup("Select Skill:", selected, MainController.skillNames);
        selectedSkillName = MainController.skillNames[selected];
        myTarget.LoadJsonFile(selectedSkillName);

        if (selectedSkillName != null)
        {
            EditorGUILayout.LabelField(string.Concat("JSON File: ", selectedSkillName));
            EditorGUILayout.PropertyField(trainingMethod, new GUIContent("Training Methods:"));

            int methodIndex = 0;
            foreach (SerializedProperty method in trainingMethod)
            {
                if (GUILayout.Button(method.displayName))
                {
                    isTrainMethodSelected[methodIndex] = !isTrainMethodSelected[methodIndex];
                }

                if (isTrainMethodSelected[methodIndex])
                {
                    foreach (SerializedProperty p in method)
                    {
                        EditorGUILayout.LabelField(p.displayName);
                    }
                }

                methodIndex++;
            }
        }

        if (GUILayout.Button("Apply"))
        {
            serializedObject.ApplyModifiedProperties();
            myTarget.saveJsonFile(); //TODO

        }
            
    }
}
