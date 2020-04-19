using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MainController))]
public class InventoryEditor : Editor
{

    long id;
    int amount;
    public override void OnInspectorGUI()
    {
        MainController mainController = (MainController)target;

        EditorGUILayout.BeginHorizontal();
        id = EditorGUILayout.LongField("ID:", id);
        amount = EditorGUILayout.IntField("Amount:", amount);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add item"))
        {
            MainController.inventory.AddItem(id, amount);
        }

        if (GUILayout.Button("Remove item"))
        {
            MainController.inventory.RemoveItem(id, amount);
        }
        EditorGUILayout.EndHorizontal();
    }
}
