using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(MonsterDropTableAdder))]
public class MonsterDropTableEditor : Editor
{
    SerializedProperty monsterDropTableHandler;
    SerializedProperty generalDropTables;

    MonsterDropTable tempDropTable;

    private string monsterName;
    private int monsterInd = 0;
    private int prevMonsterInd = 0;

    private bool JSONLoaded = false;

    void OnEnable()
    {
        monsterDropTableHandler = serializedObject.FindProperty("monsterDropTableHandler");
        generalDropTables = monsterDropTableHandler.FindPropertyRelative("generalDropTables");
    }

    public override void OnInspectorGUI()
    {
        MonsterDropTableAdder monsterDropTableAdder = (MonsterDropTableAdder)target;

        if (GUILayout.Button("Load Bosses"))
        {
            Database.LoadBosses();
        }

        monsterInd = EditorGUILayout.Popup("Select Boss:", monsterInd, Database.bossesNames.ToArray());
        monsterName = Database.bossesNames[monsterInd];
        if (monsterInd != prevMonsterInd)
        {
            JSONLoaded = false;
        }

        if (GUILayout.Button("Load JSON"))
        {
            monsterDropTableAdder.monsterDropTableHandler = JsonHandler.GetMonster(monsterName);
            serializedObject.Update();
            prevMonsterInd = monsterInd;
            JSONLoaded = true;
        }

        if (monsterName != null && JSONLoaded)
        {
            EditorGUILayout.LabelField("General Info", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(monsterDropTableHandler.FindPropertyRelative("rolls"));

            EditorGUILayout.Space(10);

            EditorGUILayout.LabelField("100% Drop and Tertiary", EditorStyles.boldLabel);
            if (GUILayout.Button("Add"))
            {
                monsterDropTableAdder.monsterDropTableHandler.generalDropTables.Add(new GeneralDropTable());
            }
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(generalDropTables, false);
            if (generalDropTables.isExpanded)
            {
                EditorGUI.indentLevel++;
                foreach (SerializedProperty table in generalDropTables)
                {
                    EditorGUILayout.PropertyField(table, false);
                    if (table.isExpanded)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.PropertyField(table.FindPropertyRelative("name"));
                        EditorGUILayout.PropertyField(table.FindPropertyRelative("numRolls"));
                        EditorGUILayout.PropertyField(table.FindPropertyRelative("lootItems"));
                        EditorGUI.indentLevel--;
                    }
                }
                EditorGUI.indentLevel--;

            }
            EditorGUI.indentLevel--;

            EditorGUILayout.Space(10);

            EditorGUILayout.LabelField("General Loot", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(monsterDropTableHandler.FindPropertyRelative("baseChance"));
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(monsterDropTableHandler.FindPropertyRelative("indexMapping"));
            EditorGUI.indentLevel--;

            if (GUILayout.Button("Add basic loot and index map"))
            {
                tempDropTable = JsonHandler.GetDropTable("temp");
                monsterDropTableAdder.monsterDropTableHandler.basicLoots = tempDropTable.basicLoots;
                monsterDropTableAdder.monsterDropTableHandler.indexMapping = tempDropTable.indexMapping;
                monsterDropTableAdder.monsterDropTableHandler.baseChance = tempDropTable.baseChance;
            }

            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(monsterDropTableHandler.FindPropertyRelative("basicLoots"));
            EditorGUI.indentLevel--;

            EditorGUILayout.LabelField("Other tables", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            if (GUILayout.Button("Add RDT"))
            {
                tempDropTable = JsonHandler.GetDropTable("rdt");
                monsterDropTableAdder.monsterDropTableHandler.monsterDropTables.Add(tempDropTable);
            }
            if (GUILayout.Button("Add Tree-Herb Seed Table"))
            {
                tempDropTable = JsonHandler.GetDropTable("tree_herb_seed");
                monsterDropTableAdder.monsterDropTableHandler.monsterDropTables.Add(tempDropTable);
            }
            if (GUILayout.Button("Add Custom"))
            {
                monsterDropTableAdder.monsterDropTableHandler.monsterDropTables.Add(new MonsterDropTable());
            }
            EditorGUILayout.PropertyField(monsterDropTableHandler.FindPropertyRelative("monsterDropTables"));
            EditorGUI.indentLevel--;

            EditorGUI.indentLevel--;
            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Apply"))
            {
                JsonHandler.SaveJsonFile(monsterDropTableAdder.monsterDropTableHandler, monsterName);
            }
        }
    }
}
