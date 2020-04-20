using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
public static class JsonHandler
{
    public static List<TrainingMethod> getTrainingMethods(string skillName)
    {
        List<TrainingMethod> trainingMethods = new List<TrainingMethod>();
        TextAsset trainingMethodsJsonFile = Resources.Load<TextAsset>(string.Concat("JSON/TrainingMethods/", skillName));
        TrainingMethodList trainingMethodList = JsonUtility.FromJson<TrainingMethodList>(trainingMethodsJsonFile.text);

        if (trainingMethodList.trainingMethodList.Count > 0)
        {
            foreach (TrainingMethod trainingMethod in trainingMethodList.trainingMethodList)
            {
                trainingMethods.Add(trainingMethod);
                //inplace sort method. fast and not expensiv

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

    public static List<int> getSkillLevels()
    {
        TextAsset skillLevelsJsonFile = Resources.Load<TextAsset>("JSON/Levels");
        SkillLevelList levels = JsonUtility.FromJson<SkillLevelList>(skillLevelsJsonFile.text);
        return levels.levels;
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
}
