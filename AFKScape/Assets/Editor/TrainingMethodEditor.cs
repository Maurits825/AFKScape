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
    int skillReqInd = 0;
    string skillReq;

    bool JSONLoaded = false;

    public static Dictionary<string, List<long>> generalItems = new Dictionary<string, List<long>>
    {
        {"template", new List<long>() { 0 } },
        { "Mining", new List<long>() { 1265, 1267, 1269, 1271, 1273, 1275, 11920, 12297, 12797, 13243, 13244, 20014, 23276, 23677, 23680, 23682 } },
        { "Woodcutting", new List<long>() { 1349, 1351, 1353, 1355, 1357, 1359, 1361, 6739, 13242, 13241, 20011, 23279, 23673, 23675 } }
    };

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
            Database.LoadSkills();
        }

        selectedSkillInd = EditorGUILayout.Popup("Select Skill:", selectedSkillInd, Database.skillNames);
        selectedSkillName = Database.skillNames[selectedSkillInd];
        if (selectedSkillInd != selectedSkillIndPrev)
        {
            JSONLoaded = false;
        }

        if (GUILayout.Button("Load JSON"))
        {
            trainingMethodAdder.LoadJsonFile(selectedSkillName);
            serializedObject.Update();
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
                        EditorGUILayout.LabelField("DropTables:", EditorStyles.boldLabel);
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

                        EditorGUI.indentLevel++;
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
                        EditorGUI.indentLevel--;
                        EditorGUILayout.Space(10);
                        EditorGUILayout.LabelField("Requirements:", EditorStyles.boldLabel);
                        EditorGUILayout.BeginHorizontal();
                        if (GUILayout.Button("Add Level Req"))
                        {
                            trainingMethodAdder.trainingMethods[methodIndex].requirements.levelRequirements.Add(new LevelRequirement(skillReq, 1));
                        }
                        skillReqInd = EditorGUILayout.Popup("Select Skill:", skillReqInd, Database.skillNames, GUILayout.ExpandWidth(true));
                        skillReq = Database.skillNames[skillReqInd];
                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.BeginHorizontal();
                        if (GUILayout.Button("Add Quest Req"))
                        {
                            trainingMethodAdder.trainingMethods[methodIndex].requirements.questIds.Add(0);
                        }
                        EditorGUILayout.LabelField("placeholder");
                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.BeginHorizontal();
                        if (GUILayout.Button("Add Item Req"))
                        {
                            trainingMethodAdder.trainingMethods[methodIndex].requirements.itemIds.Add(0);
                        }
                        EditorGUILayout.LabelField("placeholder");
                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.BeginHorizontal();
                        if (GUILayout.Button("Add General Items"))
                        {
                            trainingMethodAdder.trainingMethods[methodIndex].requirements.generalSkillItems = new List<long>(generalItems[selectedSkillName]);
                        }
                        EditorGUILayout.EndHorizontal();

                        EditorGUI.indentLevel++;
                        SerializedProperty requirements = method.FindPropertyRelative("requirements");
                        EditorGUILayout.PropertyField(requirements, false);
                        if (requirements.isExpanded)
                        {
                            EditorGUI.indentLevel++;
                            EditorGUILayout.PropertyField(requirements.FindPropertyRelative("levelRequirements"));
                            EditorGUILayout.PropertyField(requirements.FindPropertyRelative("questIds"));
                            EditorGUILayout.PropertyField(requirements.FindPropertyRelative("itemIds"));
                            EditorGUILayout.PropertyField(requirements.FindPropertyRelative("generalSkillItems"));
                            EditorGUI.indentLevel--;
                        }
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
