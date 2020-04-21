using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO will be used later for specific drop table mechanics, (cerberus and zulrah)
public class DropTableManager
{
    public static void RollResources(Dictionary<long, int> dropTableDict, TrainingMethod trainingMethod, int boostedLvl)
    {
        List<DropTable> dropTables = trainingMethod.dropTables; //TODO is this better or slower?
        for (int i = 0; i < dropTables.Count; i++)
        {
            switch (dropTables[i].tableType)
            {
                case DropTable.DropTableType.General:
                    dropTables[i].RollTable(dropTableDict);
                    break;

                case DropTable.DropTableType.Clue:
                    dropTables[i].RollTable(dropTableDict, boostedLvl);
                    break;

                case DropTable.DropTableType.Pet:
                    dropTables[i].RollTable(dropTableDict, boostedLvl);
                    break;

                default:
                    break;
            }
        }
    }

    public static Dictionary<long, int> CreateDropTableDictionary(List<DropTable> dropTables)
    {
        Dictionary<long, int> dropTableDict = new Dictionary<long, int>();
        foreach (DropTable dropTable in dropTables)
        {
            if (dropTable.tableType == DropTable.DropTableType.Clue)
            {
                foreach ((int, int[]) tuple in ClueDropTable.clueChances)
                {
                    dropTableDict.Add(tuple.Item1, 0);
                }

                dropTableDict.Add(ClueDropTable.beginnerClueId, 0);
            }
            else
            {
                foreach (DropTable.Loot loot in dropTable.lootItems)
                {
                    dropTableDict.Add(loot.id, 0);
                }
            }
        }
        return dropTableDict;
    }
}
