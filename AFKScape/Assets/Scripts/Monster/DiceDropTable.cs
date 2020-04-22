using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DiceDropTable : DropTable
{
    public DiceDropTable() : base("Dice")
    {
        tableType = DropTableType.Dice;
    }

    [Serializable]
    public new struct Loot
    {
        [ItemId]
        public long id;

        public int amountMin;
        public int amountMax;

        public Tuple<int, int> indexRange;

        public Loot(long idNum)
        {
            id = idNum;
            amountMin = 1;
            amountMax = 1;
            indexRange = new Tuple<int, int>(0, 0);
        }
    }

    public override void RollTable(Dictionary<long, int> dropTableDict)
    {
        //use dice sim values
    }
}
