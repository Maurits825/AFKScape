using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//[CustomPropertyDrawer(typeof(TableDropTable))]
public class TableDropTableDrawer : PropertyDrawer
{
    bool foldout;
    public override void OnGUI(Rect position, SerializedProperty tableDropTable, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, tableDropTable);
        //position.height = 16f;
        foldout = EditorGUI.Foldout(position, foldout, label, true);
        if (foldout)
        {
            SerializedProperty isChild = tableDropTable.FindPropertyRelative("isChild");
            EditorGUILayout.PropertyField(isChild);

            if (!isChild.boolValue)
            {
                EditorGUILayout.PropertyField(tableDropTable.FindPropertyRelative("rolls"));
            }

            EditorGUILayout.PropertyField(tableDropTable.FindPropertyRelative("baseChance"));
            EditorGUILayout.PropertyField(tableDropTable.FindPropertyRelative("indexMapping"));

            if (GUILayout.Button("Add Emtpy Table"))
            {
                ;
            }

            EditorGUILayout.PropertyField(tableDropTable.FindPropertyRelative("diceDropTables"));

            SerializedProperty tableDropTables = tableDropTable.FindPropertyRelative("tableDropTables");
            EditorGUILayout.PropertyField(tableDropTables); //TODO if count > 0 display
        }

        EditorGUI.EndProperty();
    }
}
