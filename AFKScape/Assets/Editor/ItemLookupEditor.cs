using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ItemLookup))]
public class ItemLookupEditor : Editor
{

    string itemName;
    string status;

    List<(long, string, string)> itemList = new List<(long, string, string)>();

    SerializedProperty filterType;

    void OnEnable()
    {
        filterType = serializedObject.FindProperty("filterType");
    }

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

        if (GUILayout.Button("Load Items"))
        {
            Database.LoadItems();
        }

        EditorGUILayout.LabelField("Status: " + status, EditorStyles.boldLabel);

        itemName = EditorGUILayout.TextField("Name:", itemName);
        EditorGUILayout.PropertyField(filterType);

        if (GUILayout.Button("Get ID"))
        {
            itemList = itemLookup.GetItemId(itemName);
        }

        if (itemList.Count > 0)
        {
            foreach ((long, string, string) item in itemList)
            {
                EditorGUILayout.BeginVertical("box");
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Name: " + item.Item2);
                EditorGUILayout.LabelField("Extra info: " + item.Item3);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.LabelField("ID: " + item.Item1.ToString());
                EditorGUILayout.EndVertical();


                EditorGUILayout.Space(3);
            }
        }
        serializedObject.ApplyModifiedProperties();
    }
}
