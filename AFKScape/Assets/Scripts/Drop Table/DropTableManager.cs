using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO will be used later for specific drop table mechanics, (cerberus and zulrah)
public class DropTableManager
{
    public static List<(long, int)> RollResources(TrainingMethod trainingMethod, int boostedLvl)
    {
        List<(long, int)> retItemList = new List<(long, int)>();

        long itemId;
        int amount;

        for (int i = 0; i < trainingMethod.dropTables.Count; i++)
        {
            switch (trainingMethod.dropTables[i].tableType)
            {
                case DropTable.DropTableType.General:
                    List<(long, int)> itemList = trainingMethod.dropTables[i].RollTable();

                    if (itemList.Count > 0)
                    {
                        foreach ((long, int) item in itemList)
                        {
                            retItemList.Add((item.Item1, item.Item2));
                        }
                    }
                    break;

                case DropTable.DropTableType.Clue:
                    (itemId, amount) = trainingMethod.dropTables[i].RollTable(boostedLvl);
                    retItemList.Add((itemId, amount));
                    break;

                case DropTable.DropTableType.Pet:
                    (itemId, amount) = trainingMethod.dropTables[i].RollTable(boostedLvl);
                    retItemList.Add((itemId, amount));
                    break;

                default:
                    break;
            }
        }

        return retItemList;
    }
}
