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
    int level;

    enum StorageType
    {
        Bank,
        Inventory,
    }

    StorageType storageType;
    Storage storageRef;

    float timeConstantGain;
    int lvl99 = 13034431;
    List<string> combatSkills = new List<string>()
    {
        "Attack",
        "Strength",
        "Defence",
        "Ranged",
        "Magic",
        "Hitpoints"
    };

    MainController mainController;
    SkillsController skillsController;

    public override void OnInspectorGUI()
    {
        mainController = (MainController)target;
        skillsController = mainController.skillsController;

        EditorGUILayout.LabelField("Constants", EditorStyles.boldLabel);
        timeConstantGain = EditorGUILayout.Slider("Gain (log scale)", timeConstantGain, 1, 5);
        MainController.timeConstant = (1.0F / (60.0F * 60.0F)) * Mathf.Pow(10, timeConstantGain);

        EditorGUILayout.Space(10);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Inventory / Bank", EditorStyles.boldLabel);
        storageType = (StorageType)EditorGUILayout.EnumPopup(storageType);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical();

        switch (storageType)
        {
            case StorageType.Bank:
                storageRef = mainController.bank;
                break;
            case StorageType.Inventory:
                storageRef = mainController.inventory;
                break;
            default:
                break;
        }

        id = EditorGUILayout.LongField("ID:", id);
        if (GUILayout.Button("Add item"))
        {
            storageRef.AddItem(id, amount);
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical();
        amount = EditorGUILayout.IntField("Amount:", amount);
        if (GUILayout.Button("Remove item"))
        {
            storageRef.RemoveItem(id, amount);
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Fill random"))
        {
            for (int i = 0; i < 28; i++)
            {
                int idRand = Random.Range(1, 20000);
                while (!Database.items.ContainsKey(idRand))
                {
                    idRand++;
                }
                int amountRand = Random.Range(1, 100);
                storageRef.AddItem(idRand, amountRand);
            }
        }

        if (GUILayout.Button("Remove all"))
        {
            storageRef.RemoveAll();
        }

        EditorGUILayout.Space(10);

        EditorGUILayout.LabelField("Experience", EditorStyles.boldLabel);
        selectedSkillInd = EditorGUILayout.Popup("Select Skill:", selectedSkillInd, Database.skillNames);
        skillName = Database.skillNames[selectedSkillInd];
        experience = EditorGUILayout.IntField("Experience:", experience);

        if (GUILayout.Button("Add xp"))
        {
            Skill skill = skillsController.skillsClasses[skillName];
            skill.xpFloat += experience;

            SimEvents(skill, skillName);
        }

        EditorGUILayout.Space(10);

        EditorGUILayout.LabelField("Max Combat", EditorStyles.boldLabel);
        if (GUILayout.Button("Max combat"))
        {
            foreach (string skills in combatSkills)
            {
                Skill skill = skillsController.skillsClasses[skills];
                skill.xpFloat = lvl99;

                SimEvents(skill, skills);
            }
        }

        EditorGUILayout.Space(10);

        EditorGUILayout.LabelField("Set level in skill", EditorStyles.boldLabel);
        selectedSkillInd = EditorGUILayout.Popup("Select Skill:", selectedSkillInd, Database.skillNames);
        skillName = Database.skillNames[selectedSkillInd];
        level = EditorGUILayout.IntField("Level:", level);
        if (GUILayout.Button("Level # in skill"))
        {
            Skill skill = skillsController.skillsClasses[skillName];
            skill.xpFloat = Database.experienceTable[level-1];

            SimEvents(skill, skillName);
        }

        EditorGUILayout.Space(10);

        EditorGUILayout.LabelField("Max Level", EditorStyles.boldLabel);
        if (GUILayout.Button("Max Level"))
        {
            foreach (string skills in skillsController.skillsClasses.Keys)
            {
                Skill skill = skillsController.skillsClasses[skills];
                skill.xpFloat = lvl99;

                SimEvents(skill, skills);
            }
        }

        EditorGUILayout.Space(10);

        if (GUILayout.Button("Reset Levels"))
        {
            foreach (string skills in skillsController.skillsClasses.Keys)
            {
                Skill skill = skillsController.skillsClasses[skills];
                skill.xpFloat = 0;
                skill.boostedLevel = 1;

                SimEvents(skill, skills);
            }
        }
    }

    private void SimEvents(Skill skill, string skillName)
    {
        EventManager.Instance.SkillButtonClicked(skillName);
        EventManager.Instance.XpGained(skill.xp);
        int newLvl = SkillsController.GetLevel(skill.xp);
        skill.currentLevel = newLvl;
        if (skill.boostedLevel < skill.currentLevel)
        {
            skill.boostedLevel = skill.currentLevel;
        }
        int totalLvl = skillsController.GetTotalLevel();
        EventManager.Instance.LevelUp(skill.skillName, skill.currentLevel, totalLvl);
    }
}
