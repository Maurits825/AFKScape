using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO will be used later for specific drop table mechanics, (cerberus and zulrah)
public class DropTableManager
{
    public static List<(long, int)> RollResources(TrainingMethod trainingMethod, int boostedLvl)
    {
        List<(long, int)> retItemList = new List<(long, int)>();

        List<DropTable> dropTables = trainingMethod.dropTables; //TODO is this better or slower?
        for (int i = 0; i < dropTables.Count; i++)
        {
            switch (dropTables[i].tableType)
            {
                case DropTable.DropTableType.General:
                    dropTables[i].RollTable(retItemList);
                    break;

                case DropTable.DropTableType.Clue:
                    dropTables[i].RollTable(retItemList, boostedLvl);
                    break;

                case DropTable.DropTableType.Pet:
                    dropTables[i].RollTable(retItemList, boostedLvl);
                    break;

                default:
                    break;
            }
        }

        return retItemList;
    }
}
