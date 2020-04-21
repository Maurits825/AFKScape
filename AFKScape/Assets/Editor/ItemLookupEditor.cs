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
    SerializedProperty idList;

    bool asButton = false;

    void OnEnable()
    {
        filterType = serializedObject.FindProperty("filterType");
        idList = serializedObject.FindProperty("idList");
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

        asButton = EditorGUILayout.Toggle("As buttons", asButton);
        
        if (GUILayout.Button("Get ID"))
        {
            itemList = itemLookup.GetItemId(itemName);
        }

        if (itemList.Count > 0)
        {
            if (asButton)
            {
                foreach ((long, string, string) item in itemList)
                {
                    if (GUILayout.Button("Name: " + item.Item2 + ", ID: " + item.Item1.ToString()))
                    {
                        itemLookup.idList.Add(item.Item1);
                    }
                    EditorGUILayout.Space(3);
                }
            }
            else
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
        }

        if (asButton)
        {
            if (GUILayout.Button("Print"))
            {
                string toPrint = "";
                for (int i = 0; i < itemLookup.idList.Count; i++)
                {
                    toPrint += ", " + itemLookup.idList[i];
                }
                Debug.Log(toPrint);
            }
            EditorGUILayout.PropertyField(idList);
        }
        serializedObject.ApplyModifiedProperties();
    }
}
