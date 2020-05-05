using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

public static class JsonHandler
{
    public static List<TrainingMethod> GetTrainingMethods(string skillName)
    {
        List<TrainingMethod> trainingMethods = new List<TrainingMethod>();
        TextAsset trainingMethodsJsonFile = Resources.Load<TextAsset>(string.Concat("JSON/TrainingMethods/", skillName));
        TrainingMethodList trainingMethodList = JsonUtility.FromJson<TrainingMethodList>(trainingMethodsJsonFile.text);

        if (trainingMethodList.trainingMethodList.Count > 0)
        {
            foreach (TrainingMethod trainingMethod in trainingMethodList.trainingMethodList)
            {
                trainingMethods.Add(trainingMethod);

                for (int i = 0; i < trainingMethod.dropTables.Count; i++)
                {
                    switch (trainingMethod.dropTables[i].tableType)
                    {
                        case DropTable.DropTableType.General:
                            trainingMethod.dropTables[i] = new GeneralDropTable(trainingMethod.dropTables[i]);
                            break;

                        case DropTable.DropTableType.Clue:
                            trainingMethod.dropTables[i] = new ClueDropTable(trainingMethod.dropTables[i]);
                            break;

                        case DropTable.DropTableType.Pet:
                            trainingMethod.dropTables[i] = new PetDropTable(trainingMethod.dropTables[i]);
                            break;

                        default:
                            break;
                    }
                }
            }
        }

        return trainingMethods;
    }

    [Serializable]
    public class SkillLevelList
    {
        public List<int> levels;
    }

    public static List<int> GetSkillLevels()
    {
        TextAsset skillLevelsJsonFile = Resources.Load<TextAsset>("JSON/Levels");
        SkillLevelList levels = JsonUtility.FromJson<SkillLevelList>(skillLevelsJsonFile.text);
        return levels.levels;
    }

    [Serializable]
    public class JsonHelper
    {
        public List<string> data;
    }

    public static string[] GetLoadedSkills()
    {
        TextAsset JsonString = Resources.Load<TextAsset>("JSON/Skills");
        JsonHelper jsonHelperSkills = JsonUtility.FromJson<JsonHelper>(JsonString.text);
        return jsonHelperSkills.data.ToArray();
    }

    public static ItemList GetLoadedItems()
    {
        TextAsset JsonString = Resources.Load<TextAsset>(string.Concat("JSON/", "Items"));
        return JsonUtility.FromJson<ItemList>(JsonString.text);
    }

    public static List<string> GetBossesNames()
    {
        TextAsset JsonString = Resources.Load<TextAsset>("JSON/Bosses");
        JsonHelper jsonHelperBosses = JsonUtility.FromJson<JsonHelper>(JsonString.text);
        return jsonHelperBosses.data;
    }

    public static MonsterDropTableHandler GetMonster(string monsterName)
    {
        TextAsset monsterDropTableJsonFile = Resources.Load<TextAsset>(string.Concat("JSON/MonsterDropTable/", monsterName));
        return JsonUtility.FromJson<MonsterDropTableHandler>(monsterDropTableJsonFile.text);
    }

    public static MonsterDropTable GetDropTable(string name)
    {
        TextAsset dropTableJsonFile = Resources.Load<TextAsset>(string.Concat("JSON/MonsterDropTable/", name));
        return JsonUtility.FromJson<MonsterDropTable>(dropTableJsonFile.text);
    }

    public static void SaveJsonFile(List<TrainingMethod> tMethodList, string selectedSkillName)
    {
        tMethodList.Sort((x, y) => x.requirements.levelRequirements[0].levelReq.CompareTo(y.requirements.levelRequirements[0].levelReq));
        TrainingMethodList trainingMethodList = new TrainingMethodList
        {
            trainingMethodList = tMethodList
        };

        string JSONString = JsonUtility.ToJson(trainingMethodList);

        //TODO better way for path?
        File.WriteAllText(string.Concat(Application.dataPath, "/Resources/JSON/TrainingMethods/", selectedSkillName, ".json"), JSONString);
        AssetDatabase.Refresh();
    }

    public static void SaveJsonFile(MonsterDropTableHandler monsterDropTable, string monsterName)
    {
        string JSONString = JsonUtility.ToJson(monsterDropTable);

        //TODO better way for path?
        File.WriteAllText(string.Concat(Application.dataPath, "/Resources/JSON/MonsterDropTable/", monsterName, ".json"), JSONString);
        AssetDatabase.Refresh();
    }
}
