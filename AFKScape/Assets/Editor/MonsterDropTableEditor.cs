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

    MonsterDropTable tempDiceDropTable;

    private string monsterName;

    ReorderableList reorderableList;

    bool foldout;

    int tableDepth;

    int index = 0;

    void OnEnable()
    {
        tableDepth = 0;
        monsterDropTableHandler = serializedObject.FindProperty("monsterDropTableHandler");
        generalDropTables = monsterDropTableHandler.FindPropertyRelative("generalDropTables");
        //diceDropTable = serializedObject.FindProperty("monsterDropTable.diceDropTable");

        //reorderableList = new ReorderableList(serializedObject, diceDropTable, true, true, true, true);

        //reorderableList.drawHeaderCallback = DrawHeader;
    }

    public override void OnInspectorGUI()
    {
        MonsterDropTableAdder monsterDropTableAdder = (MonsterDropTableAdder)target;
        //EditorGUILayout.PropertyField(monsterDropTableHandler);
        
        monsterName = EditorGUILayout.TextField("Name:", monsterName);
        if (GUILayout.Button("Load monster"))
        {
            monsterDropTableAdder.monsterDropTableHandler = JsonHandler.GetMonster(monsterName);
        }

        EditorGUILayout.LabelField("General Info", EditorStyles.boldLabel);
        //EditorGUILayout.PropertyField(monsterDropTableHandler.FindPropertyRelative("name"));
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
            tempDiceDropTable = JsonHandler.GetDropTable();
            monsterDropTableAdder.monsterDropTableHandler.basicLoots = tempDiceDropTable.basicLoots;
            monsterDropTableAdder.monsterDropTableHandler.indexMapping = tempDiceDropTable.indexMapping;
            monsterDropTableAdder.monsterDropTableHandler.baseChance = tempDiceDropTable.baseChance;
        }

        EditorGUI.indentLevel++;
        EditorGUILayout.PropertyField(monsterDropTableHandler.FindPropertyRelative("basicLoots"));
        EditorGUI.indentLevel--;

        EditorGUILayout.LabelField("Other tables", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;
        EditorGUILayout.PropertyField(monsterDropTableHandler.FindPropertyRelative("monsterDropTables"));
        EditorGUI.indentLevel--;
        /*

        EditorGUILayout.LabelField("100% Drop and Tertiary", EditorStyles.boldLabel);
        //EditorGUILayout.PropertyField(monsterDropTable.FindPropertyRelative("rolls"));
        EditorGUILayout.PropertyField(monsterDropTable.FindPropertyRelative("generalDropTable"));

        EditorGUILayout.Space(10);

        EditorGUILayout.LabelField("Uniques, Other and RDT", EditorStyles.boldLabel);
        //EditorGUILayout.PropertyField(monsterDropTable.FindPropertyRelative("baseChance"));
        //EditorGUILayout.PropertyField(monsterDropTable.FindPropertyRelative("indexMapping")); //TODO custom?

        //reorderableList.DoLayoutList();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add Emtpy Table"))
        {
            //monsterDropTableAdder.monsterDropTable.diceDropTable.Add(new DiceDropTable());
        }

        if (GUILayout.Button("Add Json Table"))
        {
            tempDiceDropTable = JsonHandler.GetDiceDropTable();
            //monsterDropTableAdder.monsterDropTable.diceDropTable.Add(tempDiceDropTable);
        }
        EditorGUILayout.EndHorizontal();

        SerializedProperty tableDropTable = monsterDropTable.FindPropertyRelative("tableDropTable");
        DrawTableDropTable(monsterDropTableAdder, tableDropTable);
        //EditorGUILayout.PropertyField(tableDropTable, true);
        /*
        if (tableDropTable.isExpanded)
        {
            EditorGUI.indentLevel++;

            foreach (SerializedProperty table in diceDropTable)
            {
                EditorGUILayout.PropertyField(table.FindPropertyRelative("name"));

                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(table.FindPropertyRelative("lootList"));
                EditorGUI.indentLevel--;
            }

            EditorGUI.indentLevel--;
        }*/

        EditorGUI.indentLevel--;
        serializedObject.ApplyModifiedProperties();

        if (GUILayout.Button("Apply"))
        {
            JsonHandler.SaveJsonFile(monsterDropTableAdder.monsterDropTableHandler, monsterName);

        }
    }

    void DrawHeader(Rect rect)
    {
        string name = "Re-order drop tables";
        EditorGUI.LabelField(rect, name);
    }

    void DrawTableDropTable(MonsterDropTableAdder monsterDropTableAdder, SerializedProperty tableDropTable)
    {
        tableDepth++;        
        EditorGUILayout.PropertyField(tableDropTable, false);

        if (tableDropTable.isExpanded)
        {
            SerializedProperty isChild = tableDropTable.FindPropertyRelative("isChild");
            EditorGUILayout.PropertyField(isChild);

            if (!isChild.boolValue)
            {
                EditorGUILayout.PropertyField(tableDropTable.FindPropertyRelative("rolls"));
            }

            EditorGUILayout.PropertyField(tableDropTable.FindPropertyRelative("baseChance"));
            EditorGUILayout.PropertyField(tableDropTable.FindPropertyRelative("indexMapping"));

            index = EditorGUILayout.IntField("Index: ", index);
            if (GUILayout.Button("Add Json Dice Table"))
            {
                //i dont even know, too deep to give up
                if (tableDepth == 1)
                {
                   // monsterDropTableAdder.monsterDropTableHandler.tableDropTable.diceDropTables.Add(JsonHandler.GetDiceDropTable());
                }
                else if (tableDepth == 2)
                {
                    //monsterDropTableAdder.monsterDropTableHandler.tableDropTable.tableDropTables[index].diceDropTables.Add(JsonHandler.GetDiceDropTable());
                }
            }

            EditorGUILayout.PropertyField(tableDropTable.FindPropertyRelative("diceDropTables"));

            //SerializedProperty tableDropTables = tableDropTable.FindPropertyRelative("tableDropTables");
            //EditorGUILayout.PropertyField(tableDropTables); //TODO if count > 0 display
            if (tableDepth < 5)
            {
                SerializedProperty tableList = tableDropTable.FindPropertyRelative("tableDropTables");
                DrawTableDropTable(monsterDropTableAdder, tableList.GetArrayElementAtIndex(0));
            }
        }

        tableDepth--;
    }
}