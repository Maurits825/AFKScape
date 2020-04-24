using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BossesController : MonoBehaviour
{
    Monster Zulrah;
    Monster Vorkath;

    public static Inventory inventory = new Inventory(28);
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
        AddItemsToInventory(dropTableDict);
    }

    public void VorkathKillTest()
    {
        dropTableDict = Vorkath.monsterDropTableHandler.CreateDropTableDictionary();
        Vorkath.monsterDropTableHandler.RollTable(dropTableDict);
        AddItemsToInventory(dropTableDict);
    }

    //TODO this can be move to inv?
    private void AddItemsToInventory(Dictionary<long, int> items)
    {
        foreach (long id in items.Keys.ToList())
        {
            if (items[id] > 0)
            {
                inventory.AddItem(id, items[id]);
                items[id] = 0;
            }
        }
    }
}
