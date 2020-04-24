using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveHandler : MonoBehaviour
{
   
    public void SaveGame()
    {
        MainController mainController = FindObjectOfType<MainController>();
        SaveData saveData = new SaveData();
        foreach (Skill skill in mainController.skillsClasses.Values)
        {
            saveData.SetSkills(skill);
        }
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.dataPath + "/SaveData/MySaveData.dat");
        bf.Serialize(file, saveData);
        file.Close();
    }

    public void LoadGame()
    {
        if (File.Exists(Application.dataPath + "/SaveData/MySaveData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.dataPath + "/SaveData/MySaveData.dat", FileMode.Open); //better way to do filepaths?
            SaveData saveData = (SaveData)bf.Deserialize(file);
            file.Close();
            Dictionary<string, float> skills = new Dictionary<string, float>();
            MainController mainController = FindObjectOfType<MainController>();
            skills = saveData.GetSkills();
            foreach (string skillName in skills.Keys)
            {
                Skill skill = mainController.skillsClasses[skillName];
                skill.xpFloat = skills[skillName];

                EventManager.Instance.SkillClicked(skillName);
                EventManager.Instance.XpGained(skill.xp);
                int newLvl = MainController.GetLevel(skill.xp);
                skill.currentLevel = newLvl;
                int totalLvl = mainController.GetTotalLevel();
                EventManager.Instance.LevelUp(skill.skillName, skill.currentLevel, totalLvl);
            }
        }
        else
        {
            //TODO print error message
        }
    }
}
