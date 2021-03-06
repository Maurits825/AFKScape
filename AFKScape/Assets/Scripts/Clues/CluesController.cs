﻿using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;

public class CluesController
{
    public Dictionary<string, Monster> cluesClasses = new Dictionary<string, Monster>();
    private Dictionary<long, BigInteger> dropTableDict = new Dictionary<long, BigInteger>();
    private Monster currentClue;

    private Inventory inventory;
    private Bank bank;

    private string selectedClueName;

    private float actionCount;

    private Dictionary<string, (int, int)> rolls = new Dictionary<string, (int, int)>
    {
        { "Clue scroll (Beginner)", (1, 3) },
        { "Clue scroll (Easy)", (2, 4) },
        { "Clue scroll (Medium)", (3, 5) },
        { "Clue scroll (Hard)", (4, 6) },
        { "Clue scroll (Elite)", (4, 6) },
        { "Clue scroll (Master)", (5, 7) },
    };

    public CluesController(Inventory inventory, Bank bank)
    {
        Initialize(inventory, bank);
    }

    private void Initialize(Inventory inventory, Bank bank)
    {
        this.inventory = inventory;
        this.bank = bank;

        SubscribeEvents();
        InitCluesClasses();
    }

    public void InitCluesClasses()
    {
        foreach (string clue in Database.cluesNames)
        {
            Monster newClue = new Monster(clue);
            newClue.Initialize();
            cluesClasses.Add(clue, newClue);
        }
    }

    public void SubscribeEvents()
    {
        EventManager.Instance.OnClueClicked += OnClueSelected;
    }

    public void Operate()
    {
        if (!string.IsNullOrEmpty(selectedClueName))
        {
            ClueGameLoop(currentClue, Time.deltaTime);
        }
    }

    public void ClueGameLoop(Monster monster, float deltaTime)
    {
        //float kcRate = monster.GetKillCountRate(); //TODO
        float kcRate = 10000;
        float currentDeltaTime = deltaTime;
        float actionIncrement = kcRate * currentDeltaTime * MainController.timeConstant;
        actionCount += actionIncrement;

        int actionDone = 0;
        while (actionCount >= 1.0F)
        {
            actionCount -= 1.0F;
            actionDone++;

            monster.monsterDropTableHandler.rolls = GetRandomRoll();
            monster.KillBoss(dropTableDict);
            monster.killCount++;
            EventManager.Instance.ClueCompleted(monster.killCount);
        }

        bank.AddMultipleItems(dropTableDict);
        EventManager.Instance.UpdateLastLoot(dropTableDict);
        ClearDropTable(dropTableDict);
    }

    public void OnClueSelected(string clueName)
    {
        selectedClueName = clueName;

        if (!string.IsNullOrEmpty(selectedClueName))
        {
            currentClue = cluesClasses[selectedClueName];
            dropTableDict = currentClue.GetDropTableDict();
        }
    }

    private void ClearDropTable(Dictionary<long, BigInteger> dropTable)
    {
        foreach (long id in dropTable.Keys.ToList())
        {
            dropTable[id] = 0;
        }
    }

    private int GetRandomRoll()
    {
        return UnityEngine.Random.Range(rolls[selectedClueName].Item1, rolls[selectedClueName].Item2 + 1);
    }
}
