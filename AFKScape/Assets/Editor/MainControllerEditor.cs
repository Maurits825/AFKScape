using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MainController))]
public class MainControllerEditor : Editor
{
    long id;
    int amount;

    public override void OnInspectorGUI()
    {
        MainController mainController = (MainController)target;
       
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical();

        id = EditorGUILayout.LongField("ID:", id);
        if (GUILayout.Button("Add item"))
        {
            MainController.inventory.AddItem(id, amount);
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical();
        amount = EditorGUILayout.IntField("Amount:", amount);
        if (GUILayout.Button("Remove item"))
        {
            MainController.inventory.RemoveItem(id, amount);
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Remove all"))
        {
            MainController.inventory.RemoveAll();
        }
    }
}
