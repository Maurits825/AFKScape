using System.Collections.Generic;
using System.Numerics;

//TODO will be used later for specific drop table mechanics, (cerberus and zulrah)
public static class DropTableManager
{
    public static void RollResources(Dictionary<long, BigInteger> dropTableDict, TrainingMethod trainingMethod, int boostedLvl)
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

    public static Dictionary<long, BigInteger> CreateDropTableDictionary(List<DropTable> dropTables)
    {
        Dictionary<long, BigInteger> dropTableDict = new Dictionary<long, BigInteger>();
        foreach (DropTable dropTable in dropTables)
        {
            if (dropTable.tableType == DropTable.DropTableType.Clue)
            {
                foreach ((int, int[]) tuple in ClueDropTable.ClueChances)
                {
                    dropTableDict.Add(tuple.Item1, 0);
                }

                dropTableDict.Add(ClueDropTable.BeginnerClueId, 0);
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
