using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(MonsterDropTableAdder))]
public class MonsterDropTableEditor : Editor
{
    SerializedProperty monsterDropTable;
    SerializedProperty diceDropTable;
    DiceDropTable tempDiceDropTable;
    
    private string monsterName;

    ReorderableList reorderableList;

    void OnEnable()
    {
        monsterDropTable = serializedObject.FindProperty("monsterDropTable");
        diceDropTable = serializedObject.FindProperty("monsterDropTable.diceDropTable");

        reorderableList = new ReorderableList(serializedObject, diceDropTable, true, true, true, true);

        reorderableList.drawHeaderCallback = DrawHeader;
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

        reorderableList.DoLayoutList();

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
    }

    void DrawHeader(Rect rect)
    {
        string name = "Re-order drop tables";
        EditorGUI.LabelField(rect, name);
    }
}
