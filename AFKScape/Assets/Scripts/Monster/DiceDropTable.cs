using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DiceDropTable : DropTable
{
    public List<Loot> lootList;

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

        public int indexMapping;

        public Loot(long idNum)
        {
            id = idNum;
            amountMin = 1;
            amountMax = 1;
            indexMapping = 0;
        }
    }

    public override void RollTable(Dictionary<long, int> dropTableDict)
    {
        //use dice sim values
    }
}
