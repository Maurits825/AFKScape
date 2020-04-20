using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO will be used later for specific drop table mechanics, (cerberus and zulrah)
public class DropTableManager
{
    public static void RollResources(List<(long, int)> itemsBuffer, TrainingMethod trainingMethod, int boostedLvl)
    {
        List<DropTable> dropTables = trainingMethod.dropTables; //TODO is this better or slower?
        for (int i = 0; i < dropTables.Count; i++)
        {
            switch (dropTables[i].tableType)
            {
                case DropTable.DropTableType.General:
                    dropTables[i].RollTable(itemsBuffer);
                    break;

                case DropTable.DropTableType.Clue:
                    dropTables[i].RollTable(itemsBuffer, boostedLvl);
                    break;

                case DropTable.DropTableType.Pet:
                    dropTables[i].RollTable(itemsBuffer, boostedLvl);
                    break;

                default:
                    break;
            }
        }
    }
}
