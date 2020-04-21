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
            JsonHandler.LoadSkills();
        }

        selectedSkillInd = EditorGUILayout.Popup("Select Skill:", selectedSkillInd, JsonHandler.skillNames);
        selectedSkillName = JsonHandler.skillNames[selectedSkillInd];
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

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.PropertyField(method.FindPropertyRelative("name"));
                        EditorGUILayout.LabelField("Xp rate: " + trainingMethodAdder.trainingMethods[methodIndex].baseXpRate);
                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.Space(5);

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.PropertyField(method.FindPropertyRelative("baseResourceRate"));
                        //EditorGUILayout.Space(5);
                        EditorGUILayout.PropertyField(method.FindPropertyRelative("xpPerResource"));
                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.Space(10);

                        EditorGUILayout.BeginHorizontal();
                        if (GUILayout.Button("Add General"))
                        {
                            trainingMethodAdder.trainingMethods[methodIndex].dropTables.Add(new GeneralDropTable());
                        }
                        if (GUILayout.Button("Add Clue"))
                        {
                            trainingMethodAdder.trainingMethods[methodIndex].dropTables.Add(new ClueDropTable()); ;
                        }
                        if (GUILayout.Button("Add Pet"))
                        {
                            trainingMethodAdder.trainingMethods[methodIndex].dropTables.Add(new PetDropTable()); ;
                        }
                        EditorGUILayout.EndHorizontal();

                        //EditorGUILayout.PropertyField(method.FindPropertyRelative("dropTables"));
                        SerializedProperty dropTables = method.FindPropertyRelative("dropTables");
                        int tableIndex = 0;
                        foreach (SerializedProperty table in dropTables)
                        {
                            DropTable.DropTableType tableType = trainingMethodAdder.trainingMethods[methodIndex].dropTables[tableIndex].tableType;
                            if (tableType == DropTable.DropTableType.General)
                            {
                                EditorGUILayout.PropertyField(table);
                            }

                            tableIndex++;
                        }

                        tableIndex = 0;
                        foreach (SerializedProperty table in dropTables)
                        {
                            DropTable.DropTableType tableType = trainingMethodAdder.trainingMethods[methodIndex].dropTables[tableIndex].tableType;
                            if (tableType == DropTable.DropTableType.Clue)
                            {
                                EditorGUILayout.PropertyField(table, false);
                                EditorGUI.indentLevel++;
                                SerializedProperty clueLoot = table.FindPropertyRelative("lootItems");
                                if (table.isExpanded)
                                {
                                    EditorGUILayout.PropertyField(clueLoot.GetArrayElementAtIndex(0).FindPropertyRelative("baseChance"));
                                }
                                EditorGUI.indentLevel--;
                            }

                            tableIndex++;
                        }

                        tableIndex = 0;
                        foreach (SerializedProperty table in dropTables)
                        {
                            DropTable.DropTableType tableType = trainingMethodAdder.trainingMethods[methodIndex].dropTables[tableIndex].tableType;
                            if (tableType == DropTable.DropTableType.Pet)
                            {
                                EditorGUILayout.PropertyField(table, false);
                                EditorGUI.indentLevel++;
                                SerializedProperty petLoot = table.FindPropertyRelative("lootItems");
                                if (table.isExpanded)
                                {
                                    EditorGUILayout.PropertyField(petLoot.GetArrayElementAtIndex(0).FindPropertyRelative("id"));
                                    EditorGUILayout.PropertyField(petLoot.GetArrayElementAtIndex(0).FindPropertyRelative("baseChance"));
                                }
                                EditorGUI.indentLevel--;
                            }

                            tableIndex++;
                        }

                        EditorGUILayout.Space(10);
                        EditorGUILayout.BeginHorizontal();
                        if (GUILayout.Button("Add Level Requirement"))
                        {
                            trainingMethodAdder.trainingMethods[methodIndex].requirements.levelRequirements.Add(new LevelRequirement());
                        }
                        if (GUILayout.Button("Add Quest Requirement"))
                        {
                            trainingMethodAdder.trainingMethods[methodIndex].requirements.questIDs.Add(0);
                        }
                        if (GUILayout.Button("Add Item Requirement"))
                        {
                            trainingMethodAdder.trainingMethods[methodIndex].requirements.itemIDs.Add(0);
                        }
                        EditorGUILayout.EndHorizontal();
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
