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
    int maxResults;
    int resultCount;

    void OnEnable()
    {
        filterType = serializedObject.FindProperty("filterType");
        idList = serializedObject.FindProperty("idList");
        maxResults = 10;
        resultCount = 0;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("Search Database for items", MessageType.Info);
        ItemLookup itemLookup = (ItemLookup)target;
        if (Database.items.Count == 0)
        {
            status = "Empty Database";
            if (GUILayout.Button("Load Items"))
            {
                Database.LoadItems();
            }
        }
        else
        {
            status = "Database Loaded";
        }

        EditorGUILayout.LabelField("Status: " + status, EditorStyles.boldLabel);

        itemName = EditorGUILayout.TextField("Name:", itemName);
        EditorGUILayout.PropertyField(filterType);

        asButton = EditorGUILayout.Toggle("As buttons", asButton);
        maxResults = EditorGUILayout.IntSlider("Limit results: ", maxResults, 5, 100);

        if (GUILayout.Button("Get ID"))
        {
            itemList = itemLookup.GetItemId(itemName);
        }

        if (Event.current.keyCode == (KeyCode.Return))
        {
            itemList = itemLookup.GetItemId(itemName);
        }

        if (itemList.Count > 0)
        {
            resultCount = 0;
            foreach ((long, string, string) item in itemList)
            {
                if (resultCount < maxResults)
                {
                    EditorGUILayout.BeginVertical("box");
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Name: " + item.Item2, GUILayout.ExpandWidth(true), GUILayout.MinWidth(50));
                    EditorGUILayout.LabelField("Extra info: " + item.Item3, GUILayout.ExpandWidth(true), GUILayout.MinWidth(50));

                    if (asButton)
                    {
                        if (GUILayout.Button("Add", GUILayout.ExpandWidth(true), GUILayout.MinWidth(50)))
                        {
                            itemLookup.idList.Add(item.Item1);
                        }
                        EditorGUILayout.Space(3);
                    }

                    GUILayout.Box(Resources.Load<Texture>("Icons/" + item.Item1.ToString()));
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.LabelField("ID: " + item.Item1.ToString());
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.Space(3);

                    resultCount++;
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
