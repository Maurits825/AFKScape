using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MonsterDropTableAdder))]
public class MonsterDropTableEditor : Editor
{
    SerializedProperty monsterDropTable;
    DiceDropTable tempDiceDropTable;

    private string monsterName;
    void OnEnable()
    {
        monsterDropTable = serializedObject.FindProperty("monsterDropTable");
    }

    public override void OnInspectorGUI()
    {
        MonsterDropTableAdder monsterDropTableAdder = (MonsterDropTableAdder)target;
        //EditorGUILayout.PropertyField(monsterDropTable);

        monsterName = EditorGUILayout.TextField("Name:", monsterName);
        if (GUILayout.Button("Load monster"))
        {
            monsterDropTableAdder.monsterDropTable = JsonHandler.GetMonster(monsterName);
        }

        EditorGUILayout.PropertyField(monsterDropTable, false);
        EditorGUI.indentLevel++;

        EditorGUILayout.PropertyField(monsterDropTable.FindPropertyRelative("rolls"));
        EditorGUILayout.PropertyField(monsterDropTable.FindPropertyRelative("baseChance"));
        EditorGUILayout.PropertyField(monsterDropTable.FindPropertyRelative("indexMapping")); //TODO custom?

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add Emtpy Table"))
        {
            monsterDropTableAdder.monsterDropTable.diceDropTable.Add(new DiceDropTable());
        }

        if (GUILayout.Button("Add Json Table"))
        {
            tempDiceDropTable = JsonHandler.GetDiceDropTable();
            monsterDropTableAdder.monsterDropTable.diceDropTable.Add(tempDiceDropTable);
        }
        EditorGUILayout.EndHorizontal();

        SerializedProperty diceDropTable = monsterDropTable.FindPropertyRelative("diceDropTable");
        EditorGUILayout.PropertyField(diceDropTable, false);
        if (diceDropTable.isExpanded)
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
        }

        EditorGUI.indentLevel--;
        serializedObject.ApplyModifiedProperties();

        if (GUILayout.Button("Apply"))
        {
            JsonHandler.SaveJsonFile(monsterDropTableAdder.monsterDropTable, monsterName);

        }

        if (GUILayout.Button("Clear"))
        {
            monsterDropTableAdder.monsterDropTable.diceDropTable.Clear();
        }

        if (GUILayout.Button("Test Save"))
        {
            JsonHandler.SaveJsonFile(monsterDropTableAdder.monsterDropTable.diceDropTable[0], "tempTest");
        }
    }
}
