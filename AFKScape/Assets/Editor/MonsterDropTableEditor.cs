using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MonsterDropTableAdder))]
public class MonsterDropTableEditor : Editor
{
    SerializedProperty monsterDropTable;

    private string monsterName;
    void OnEnable()
    {
        monsterDropTable = serializedObject.FindProperty("monsterDropTable");
    }

    public override void OnInspectorGUI()
    {
        MonsterDropTableAdder monsterDropTableAdder = (MonsterDropTableAdder)target;
        EditorGUILayout.PropertyField(monsterDropTable);

        monsterName = EditorGUILayout.TextField("Name:", monsterName);
        if (GUILayout.Button("Load monster"))
        {
            monsterDropTableAdder.monsterDropTable = JsonHandler.GetMonster(monsterName);
        }

        serializedObject.ApplyModifiedProperties();

        if (GUILayout.Button("Apply"))
        {
            JsonHandler.SaveJsonFile(monsterDropTableAdder.monsterDropTable, monsterName);

        }
    }
}
