using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ItemLookup))]
public class ItemLookupEditor : Editor
{

    string itemName;
    long id = 0;
    string status;
    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("Search Database for items", MessageType.Info);
        ItemLookup itemLookup = (ItemLookup)target;
        if (Database.items.Count == 0)
        {
            status = "Empty Database";
        }
        else
        {
            status = "Database Loaded";
        }

        EditorGUILayout.LabelField("Status: " + status, EditorStyles.boldLabel);

        itemName = EditorGUILayout.TextField("Name:", itemName);
        EditorGUILayout.LabelField("ID: " + id.ToString());

        if (GUILayout.Button("Get ID"))
        {
            id = ItemLookup.GetItemId(itemName);
        }
    }
}