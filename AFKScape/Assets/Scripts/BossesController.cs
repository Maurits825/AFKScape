using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Numerics;

public class BossesController
{
    public Dictionary<string, Monster> bossesClasses = new Dictionary<string, Monster>();
    private Monster currentMonster;

    private Dictionary<long, BigInteger> dropTableDict = new Dictionary<long, BigInteger>();

    private Inventory inventory;
    private Bank bank;

    private string selectedBossName;

    private float actionCount;

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
            BossGameLoop(currentMonster, Time.deltaTime);
        }
    }

    public void BossGameLoop(Monster monster, float deltaTime)
    {
        //float kcRate = monster.GetKillCountRate(); //TODO
        float kcRate = 10000;
        float currentDeltaTime = deltaTime;
        float actionIncrement = kcRate * currentDeltaTime * MainController.timeConstant;
        actionCount += actionIncrement;

        int actionDone = 0;
        while (actionCount >= 1.0F)
        {
            //lvl up cmbt skills?
            actionCount -= 1.0F;
            actionDone++;

            monster.KillBoss(dropTableDict);
            monster.killCount++;
            EventManager.Instance.BossKilled(monster.killCount);
        }

        bank.AddMultipleItems(dropTableDict);
    }

    public void InitMonsterClasses()
    {
        //TODO is there a better way to add/manage these
        //could use monster id
        //TODO also this could be different, there could be a specific Zulrah class
        bossesClasses.Add("Zulrah", new Zulrah());
        bossesClasses.Add("Vorkath", new Vorkath());
    }

    public void SubscribeEvents()
    {
        EventManager.Instance.OnBossClicked += OnBossSelected;
    }

    public void OnBossSelected(string bossName)
    {
        selectedBossName = bossName;
        
        if (!string.IsNullOrEmpty(selectedBossName))
        {
            currentMonster = bossesClasses[selectedBossName];
            dropTableDict = currentMonster.GetDropTableDict();
        }
    }
}
