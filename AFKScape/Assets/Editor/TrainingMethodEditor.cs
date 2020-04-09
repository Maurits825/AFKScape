using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TrainingMethodAdder))]
public class trainingMethodEditor : Editor
{
    SerializedProperty trainingMethod;
    private string selectedSkillName;
    public List<bool> isTrainMethodSelected;
    int selectedSkillIndPrev = 0;
    int selectedSkillInd = 0;

    bool JSONLoaded = false;

    string newResource;

    void OnEnable()
    {
        trainingMethod = serializedObject.FindProperty("trainingMethods");
    }

    public override void OnInspectorGUI()
    {
        TrainingMethodAdder trainingMethodAdder = (TrainingMethodAdder)target;

        EditorGUILayout.HelpBox("Load a JSON file from a skill", MessageType.Info);

        if (GUILayout.Button("Load Skills"))
        {
            MainController.LoadDataFromJson();
        }

        selectedSkillInd = EditorGUILayout.Popup("Select Skill:", selectedSkillInd, MainController.skillNames);
        selectedSkillName = MainController.skillNames[selectedSkillInd];
        if (selectedSkillInd != selectedSkillIndPrev)
        {
            JSONLoaded = false;
        }

        if (GUILayout.Button("Load JSON"))
        {
            trainingMethodAdder.LoadJsonFile(selectedSkillName);
            selectedSkillIndPrev = selectedSkillInd;
            JSONLoaded = true;

            isTrainMethodSelected = new List<bool>();
            for (int i = 0; i < trainingMethodAdder.trainingMethods.Count; i++)
            {
                isTrainMethodSelected.Add(false);
            }
        }
            
        if (selectedSkillName != null && JSONLoaded)
        {
            EditorGUILayout.LabelField(string.Concat("JSON File: ", selectedSkillName));
            EditorGUILayout.Space(10);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Training Methods:", EditorStyles.boldLabel);
            if (GUILayout.Button("Add"))
            {
                trainingMethodAdder.trainingMethods.Add(new TrainingMethod());
                isTrainMethodSelected.Add(false);
            }
            EditorGUILayout.EndHorizontal();

            int methodIndex = 0;
            if (trainingMethodAdder.trainingMethods.Count > 0)
            {
                foreach (SerializedProperty method in trainingMethod)
                {
                    if (GUILayout.Button(method.displayName))
                    {
                        isTrainMethodSelected[methodIndex] = !isTrainMethodSelected[methodIndex];
                    }

                    if (isTrainMethodSelected[methodIndex])
                    {
                        //This will be the main place to change/add/remove properties
                        //for example if the lootable becomes a class, would need to include children
                        EditorGUI.indentLevel++;
                        EditorGUILayout.PropertyField(method.FindPropertyRelative("name"));
                        EditorGUILayout.Space(5);
                        EditorGUILayout.PropertyField(method.FindPropertyRelative("baseXpRate"));
                        EditorGUILayout.Space(5);
                        EditorGUILayout.PropertyField(method.FindPropertyRelative("xpPerResource"));
                        EditorGUILayout.Space(5);

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.PropertyField(method.FindPropertyRelative("dropTables"));
                        if (GUILayout.Button("Add General"))
                        {
                            trainingMethodAdder.trainingMethods[methodIndex].dropTables.Add(new DropTable("General"));
                        }
                        if (GUILayout.Button("Add Clue"))
                        {
                            trainingMethodAdder.trainingMethods[methodIndex].dropTables.Add(new ClueDropTable());
                        }
                        if (GUILayout.Button("Add Pet"))
                        {
                            trainingMethodAdder.trainingMethods[methodIndex].dropTables.Add(new PetDropTable());
                        }
                        EditorGUILayout.EndHorizontal();


                        EditorGUILayout.Space(5);
                        EditorGUILayout.PropertyField(method.FindPropertyRelative("requirements"));
                        EditorGUI.indentLevel--;
                    }
                    methodIndex++;

                    EditorGUILayout.Space(3);
                }
            }
            
        }

        serializedObject.ApplyModifiedProperties();

        EditorGUILayout.Space(5);
        if (GUILayout.Button("Apply"))
        {
            trainingMethodAdder.SaveJsonFile(selectedSkillName);

        }
            
    }
}
