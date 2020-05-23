using System.Collections.Generic;
using System.Numerics;

public class Zulrah : Monster
{
    private MonsterDropTable uniqueDropTable;

    public Zulrah()
        : base("Zulrah")
    {
        GetDropTableHandler(bossName);
        CreateUniqueDropTable();
        dropTableDict = CreateDropTableDictionary(new List<MonsterDropTable>() { uniqueDropTable });
    }

    private void CreateUniqueDropTable()
    {
        uniqueDropTable = new MonsterDropTable
        {
            name = "Unique",
            weight = 0,
            baseChance = 256 * 4
        };

        MonsterDropTable.BasicLoot tanz = new MonsterDropTable.BasicLoot
        {
            id = 12922,
            weight = 1,
            amountMin = 1,
            amountMax = 1
        };
        uniqueDropTable.basicLoots.Add(tanz);

        MonsterDropTable.BasicLoot magic = new MonsterDropTable.BasicLoot
        {
            id = 12932,
            weight = 1,
            amountMin = 1,
            amountMax = 1
        };
        uniqueDropTable.basicLoots.Add(magic);

        MonsterDropTable.BasicLoot visage = new MonsterDropTable.BasicLoot
        {
            id = 12927,
            weight = 1,
            amountMin = 1,
            amountMax = 1
        };
        uniqueDropTable.basicLoots.Add(visage);

        MonsterDropTable.BasicLoot onyx = new MonsterDropTable.BasicLoot
        {
            id = 6571,
            weight = 1,
            amountMin = 1,
            amountMax = 1
        };
        uniqueDropTable.basicLoots.Add(onyx);
    }

    public override void KillBoss(Dictionary<long, BigInteger> dropTableDict)
    {
        monsterDropTableHandler.RollGeneral(dropTableDict);

        for (int r = 0; r < monsterDropTableHandler.rolls; r++)
        {
            if (!uniqueDropTable.Roll(dropTableDict))
            {
                monsterDropTableHandler.RollBasic(dropTableDict);
            }
        }
    }
}
