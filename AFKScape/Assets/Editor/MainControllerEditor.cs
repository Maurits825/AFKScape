using System.Collections.Generic;
using System.Numerics;
using UnityEditor;
using UnityEngine;

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

    enum ValueMultiplier
    {
        Ones,
        K,
        M,
        B,
        T,
    }
    ValueMultiplier valueMultiplier;

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

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.BeginVertical();
        EditorGUIUtility.labelWidth = 80;
        id = EditorGUILayout.LongField("ID:", id, GUILayout.ExpandWidth(true), GUILayout.MinWidth(100));
        if (GUILayout.Button("Add item"))
        {
            storageRef.AddItem(id, GetActualAmount(amount));
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical();
        amount = EditorGUILayout.IntField("Amount:", amount, GUILayout.ExpandWidth(true), GUILayout.MinWidth(100));
        if (GUILayout.Button("Remove item"))
        {
            storageRef.RemoveItem(id, GetActualAmount(amount));
        }
        EditorGUIUtility.labelWidth = 0;
        EditorGUILayout.EndVertical();
        valueMultiplier = (ValueMultiplier)EditorGUILayout.EnumPopup(valueMultiplier, GUILayout.Width(50));
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Fill random"))
        {
            for (int i = 0; i < 28; i++)
            {
                int idRand = Random.Range(1, 20000);
                while (true)
                {
                    if (Database.items.ContainsKey(idRand))
                    {
                        if (Database.items[idRand].duplicate || Database.items[idRand].placeholder)
                        {
                            idRand++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        idRand++;
                    }
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
            skill.xpFloat = Database.experienceTable[level - 1];

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

    private BigInteger GetActualAmount(int amount)
    {
        BigInteger actualAmount;
        switch (valueMultiplier)
        {
            case ValueMultiplier.Ones:
                actualAmount = amount;
                break;
            case ValueMultiplier.K:
                actualAmount = BigInteger.Multiply(amount, 1_000);
                break;
            case ValueMultiplier.M:
                actualAmount = BigInteger.Multiply(amount, 1_000_000);
                break;
            case ValueMultiplier.B:
                actualAmount = BigInteger.Multiply(amount, 1_000_000_000);
                break;
            case ValueMultiplier.T:
                actualAmount = BigInteger.Multiply(amount, 1_000_000_000_000);
                break;
            default:
                actualAmount = amount;
                break;
        }

        return actualAmount;
    }

    public static void CreateSLNFiles()
    {
        Debug.Log("### QualityPrepareCommand:PrepareSonarFiles - Started...");
        // We actually ask Unity to create the CSPROJ and SLN files.
        bool success = EditorApplication.ExecuteMenuItem("Assets/Open C# Project");
        Debug.Log("### QualityPrepareCommand:PrepareSonarFiles - " + (success ? "Done" : "FAILED") + ".");

        // Unsupported Version
        Debug.Log("### QualityPrepareCommand:PrepareSonarFiles - Started V2...");
        System.Type T = System.Type.GetType("UnityEditor.SyncVS,UnityEditor");
        System.Reflection.MethodInfo SyncSolution = T.GetMethod("SyncSolution", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
        SyncSolution.Invoke(null, null);
        Debug.Log("### QualityPrepareCommand:PrepareSonarFiles - Ended V2...");
    }
}
