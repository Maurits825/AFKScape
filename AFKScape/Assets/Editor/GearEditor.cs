using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Gear))]
public class GearEditor : Editor
{
    private SerializedProperty gearList;
    private Gear gear;

    private int gearIndex;
    private List<string> gearNames = new List<string>();

    void OnEnable()
    {
        if (target != null)
        {
            gear = (Gear)target;
            gear.LoadGearList();

            gearList = serializedObject.FindProperty("gearList");

            gearNames = gear.GetGearNames();
        }
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(serializedObject.FindProperty("mainControllerObj"));
        EditorGUILayout.LabelField("Equip Gear", EditorStyles.boldLabel);
        gearIndex = EditorGUILayout.Popup("Select Gear", gearIndex, gearNames.ToArray());
        if (GUILayout.Button("Equip Gear"))
        {
            gear.EquipItems(gearIndex);
        }

        EditorGUILayout.Space(10);

        EditorGUILayout.LabelField("Edit Gear JSON", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(gearList);

        serializedObject.ApplyModifiedProperties();

        if (GUILayout.Button("Save Gear"))
        {
            gear.SaveGearList();
            gear.LoadGearList();
            gearNames = gear.GetGearNames();
        }
    }
}
