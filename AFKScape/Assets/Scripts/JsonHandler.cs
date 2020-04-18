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

                for (int i = 0; i < trainingMethod.dropTables.Count; i++)
                {
                    switch (trainingMethod.dropTables[i].tableType)
                    {
                        case DropTable.DropTableType.General:
                            trainingMethod.dropTables[i] = (GeneralDropTable)trainingMethod.dropTables[i];
                            break;

                        case DropTable.DropTableType.Clue:
                            trainingMethod.dropTables[i] = (ClueDropTable)trainingMethod.dropTables[i];
                            break;

                        case DropTable.DropTableType.Pet:
                            trainingMethod.dropTables[i] = (PetDropTable)trainingMethod.dropTables[i];
                            break;

                        default:
                            break;
                    }
                }
            }
        }

        return trainingMethods;
    }

    public static void SaveJsonFile(List<TrainingMethod> tMethodList, string selectedSkillName)
    {
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
