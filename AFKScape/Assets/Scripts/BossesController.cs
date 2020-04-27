using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BossesController : MonoBehaviour
{
    Monster Zulrah;
    Monster Vorkath;

    public static Inventory inventory = new Inventory();
    private Dictionary<long, int> dropTableDict = new Dictionary<long, int>();

    // Start is called before the first frame update
    void Start()
    {
        Zulrah = new Monster("Zulrah");
        Vorkath = new Monster("Vorkath");
    }

    // Update is called once per frame
    void Update()
    {
        //KillTest();
    }

    public void ZulrahKillTest()
    {
        dropTableDict = Zulrah.monsterDropTableHandler.CreateDropTableDictionary();
        Zulrah.monsterDropTableHandler.RollTable(dropTableDict);
        inventory.AddMultipleItems(dropTableDict);
    }

    public void VorkathKillTest()
    {
        dropTableDict = Vorkath.monsterDropTableHandler.CreateDropTableDictionary();
        Vorkath.monsterDropTableHandler.RollTable(dropTableDict);
        inventory.AddMultipleItems(dropTableDict);
    }
}
