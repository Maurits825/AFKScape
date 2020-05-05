using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Numerics;

public class BossesController
{
    public Dictionary<string, Monster> bossesClasses = new Dictionary<string, Monster>();

    private Dictionary<long, BigInteger> dropTableDict = new Dictionary<long, BigInteger>();

    private Inventory inventory;
    private Bank bank;

    private string selectedBossName;
   
    public void Initialize(Inventory inventory, Bank bank)
    {
        this.inventory = inventory;
        this.bank = bank;

        SubscribeEvents();
        InitMonsterClasses();
    }

    public void Operate()
    {
        if (!string.IsNullOrEmpty(selectedBossName))
        {
            bossesClasses[selectedBossName].monsterDropTableHandler.RollTable(dropTableDict);
            bank.AddMultipleItems(dropTableDict);
        }
    }

    public void InitMonsterClasses()
    {
        //TODO is there a better way to add/manage these
        //could use monster id
        //TODO also this could be different, there could be a specific Zulrah class
        bossesClasses.Add("Zulrah", new Monster("Zulrah"));
        bossesClasses.Add("Vorkath", new Monster("Vorkath"));
    }

    public void SubscribeEvents()
    {
        EventManager.Instance.OnBossClicked += OnBossSelected;
    }

    public void OnBossSelected(string bossName)
    {
        selectedBossName = bossName;
        dropTableDict = bossesClasses[selectedBossName].monsterDropTableHandler.CreateDropTableDictionary();
    }
}
