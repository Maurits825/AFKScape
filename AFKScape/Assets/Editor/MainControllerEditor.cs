using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MainController))]
public class MainControllerEditor : Editor
{
    long id;
    int amount;

    string skillName;
    int selectedSkillInd;
    int experience;

    float timeConstantGain;

    public override void OnInspectorGUI()
    {
        MainController mainController = (MainController)target;

        EditorGUILayout.LabelField("Constants", EditorStyles.boldLabel);
        timeConstantGain = EditorGUILayout.Slider("Gain (log scale)", timeConstantGain, 1, 5);
        mainController.timeConstant = (1.0F / (60.0F * 60.0F)) * Mathf.Pow(10, timeConstantGain);

        EditorGUILayout.Space(10);

        EditorGUILayout.LabelField("Inventory", EditorStyles.boldLabel);
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

        EditorGUILayout.Space(10);

        EditorGUILayout.LabelField("Experience", EditorStyles.boldLabel);
        selectedSkillInd = EditorGUILayout.Popup("Select Skill:", selectedSkillInd, Database.skillNames);
        skillName = Database.skillNames[selectedSkillInd];
        experience = EditorGUILayout.IntField("Experience:", experience);

        if (GUILayout.Button("Add xp"))
        {
            Skill skill = mainController.skillsClasses[skillName];
            skill.xpFloat += experience;

            //sim events
            EventManager.Instance.SkillClicked(skillName);
            EventManager.Instance.XpGained(skill.xp);
            int newLvl = MainController.GetLevel(skill.xp);
            skill.currentLevel = newLvl;
            int totalLvl = mainController.GetTotalLevel();
            EventManager.Instance.LevelUp(skill.skillName, skill.currentLevel, totalLvl);
        }
    }
}
