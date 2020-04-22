using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MonsterDropTableAdder))]
public class MonsterDropTableEditor : Editor
{
    SerializedProperty monsterDropTable;
    void OnEnable()
    {
        monsterDropTable = serializedObject.FindProperty("monsterDropTable");
    }

    public override void OnInspectorGUI()
    {
        MonsterDropTableAdder monsterDropTableAdder = (MonsterDropTableAdder)target;
        EditorGUILayout.PropertyField(monsterDropTable);

        serializedObject.ApplyModifiedProperties();
    }
}
