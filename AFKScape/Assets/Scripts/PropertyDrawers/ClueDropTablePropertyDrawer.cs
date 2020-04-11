using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//[CustomPropertyDrawer(typeof(ClueDropTable))]
public class ClueDropTablePropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        //EditorGUI.PropertyField(position, property);
        EditorGUI.PropertyField(position, property.FindPropertyRelative("lootItems[0].baseChance"));
        EditorGUI.EndProperty();
    }
}
