﻿using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;

public class SkillsController
{
    public Dictionary<string, Skill> skillsClasses = new Dictionary<string, Skill>();
    private Skill selectedSkill;
    private TrainingMethod selectedTrainingMethod; //TODO this
    private bool isTrainingMethodSelected = false;

    private float actionCount;

    private static readonly int MaxLvl = 126;

    private Dictionary<long, BigInteger> dropTableDict = new Dictionary<long, BigInteger>();

    private Inventory inventory;
    private Bank bank;

    public SkillsController(Inventory inventory, Bank bank)
    {
        Initialize(inventory, bank);
    }

    private void Initialize(Inventory inventory, Bank bank)
    {
        this.inventory = inventory;
        this.bank = bank;

        InitSkillClasses();
        SubscribeEvents();
        EventManager.Instance.LevelUp("Fishing", 1, GetTotalLevel()); //TODO fix this to update total level at start
    }

    public void Operate()
    {
        if (selectedSkill != null && isTrainingMethodSelected)
        {
            SkillGameLoop(selectedTrainingMethod, selectedSkill, Time.deltaTime);
        }
    }

    public void InitSkillClasses()
    {
        //combat training not included
        skillsClasses.Add("Agility", new Agility());
        skillsClasses.Add("Attack", new Attack());
        skillsClasses.Add("Strength", new Strength());
        skillsClasses.Add("Defence", new Defence());
        skillsClasses.Add("Magic", new Magic());
        skillsClasses.Add("Hitpoints", new Hitpoints());
        skillsClasses.Add("Ranged", new Ranged());
        skillsClasses.Add("Construction", new Construction());
        skillsClasses.Add("Cooking", new Cooking());
        skillsClasses.Add("Crafting", new Crafting());
        skillsClasses.Add("Farming", new Farming());
        skillsClasses.Add("Firemaking", new Firemaking());
        skillsClasses.Add("Fishing", new Fishing());
        skillsClasses.Add("Fletching", new Fletching());
        skillsClasses.Add("Herblore", new Herblore());
        skillsClasses.Add("Hunter", new Hunter());
        skillsClasses.Add("Mining", new Mining());
        skillsClasses.Add("Prayer", new Prayer());
        skillsClasses.Add("Runecraft", new Runecraft());
        skillsClasses.Add("Smithing", new Smithing());
        skillsClasses.Add("Thieving", new Thieving());
        skillsClasses.Add("Woodcutting", new Woodcutting());

        //TODO foreach (skill in skillsClasses) EventManager.levelup ? for ui lvls and total lvl
    }

    public void SubscribeEvents()
    {
        EventManager.Instance.OnSkillButtonClicked += SkillButtonClicked;
        EventManager.Instance.OnTrainingMethodClicked += SetTrainingMethod;
        EventManager.Instance.OnMainTabClicked += OnTabClicked;
    }

    public void SkillGameLoop(TrainingMethod trainingMethod, Skill skill, float deltaTime)
    {
        float currentDeltaTime = deltaTime;
        float actionIncrement = skill.GetResourceRate(trainingMethod.baseResourceRate) * currentDeltaTime * MainController.timeConstant;
        actionCount += actionIncrement;

        int actionDone = 0;
        while (actionCount >= 1.0F)
        {
            skill.xpFloat += trainingMethod.xpPerResource;
            actionCount -= 1.0F;
            actionDone++;

            EventManager.Instance.XpGained(skill.xp);

            //TODO handle multiple skill earning xp
            //foreach (skill in trainingMethod.additionSkills)
            // skill.xp += xpPerResource
            // raise xp event? --> eventManager.xpgained
            ////if (getlevel(xp) != skill.lvl) --> eventManager.levelup

            DropTableManager.RollResources(dropTableDict, trainingMethod, skill.boostedLevel);

            //TODO remove consumables

            if (skill.xp >= skill.xpNextLvl)
            {
                int newLvl = GetLevel(skill.xp);
                skill.xpNextLvl = Database.experienceTable[newLvl];
                float deltaTimePerAction = currentDeltaTime / actionIncrement;
                currentDeltaTime = actionCount * deltaTimePerAction;
                actionDone = 0;

                skill.currentLevel = newLvl;
                if (skill.boostedLevel < skill.currentLevel)
                {
                    skill.boostedLevel = skill.currentLevel;
                }

                actionIncrement = skill.GetResourceRate(trainingMethod.baseResourceRate) * currentDeltaTime * MainController.timeConstant;
                actionCount = actionIncrement;

                int totalLvl = GetTotalLevel();
                EventManager.Instance.LevelUp(skill.skillName, skill.currentLevel, totalLvl);
            }
        }

        bank.AddMultipleItems(dropTableDict);
        EventManager.Instance.UpdateLastLoot(dropTableDict);
        ClearDropTable(dropTableDict);
    }

    public static int GetLevel(int xp)
    {
        for (int i = 0; i < Database.experienceTable.Count; i++)
        {
            if (xp < Database.experienceTable[i])
            {
                return i;
            }
        }

        return MaxLvl;
    }

    public int GetTotalLevel()
    {
        int totalLvl = 0;
        foreach (Skill skill in skillsClasses.Values)
        {
            totalLvl += skill.currentLevel;
        }

        return totalLvl;
    }

    public bool LevelRequirement(List<LevelRequirement> levelRequirement)
    {
        for (int i = 0; i < levelRequirement.Count; i++)
        {
            string skillName = levelRequirement[i].skillName;
            if (skillsClasses[skillName].boostedLevel < levelRequirement[i].levelReq)
            {
                return false;
            }
        }

        return true;
    }

    public bool ItemRequirement(List<long> itemIds)
    {
        for (int i = 0; i < itemIds.Count; i++)
        {
            if (!inventory.Contains(itemIds[i]))
            {
                return false;
            }
        }

        return true;
    }

    public bool GeneralItemRequirement(List<long> generalItemIds)
    {
        if (generalItemIds.Count == 0)
        {
            return true;
        }

        for (int i = 0; i < generalItemIds.Count; i++)
        {
            if (inventory.Contains(generalItemIds[i]))
            {
                return true;
            }
        }

        return false;
    }

    public bool QuestRequirement(List<int> questIds)
    {
        //TODO implement quest requirements
        return true;
    }

    public bool CheckRequirement(TrainingMethod trainingMethod)
    {
        if (!LevelRequirement(trainingMethod.requirements.levelRequirements))
        {
            EventManager.Instance.ShowPopUpMsg("Need lvl x");
            return false;
        }

        if (!ItemRequirement(trainingMethod.requirements.itemIds))
        {
            EventManager.Instance.ShowPopUpMsg("Need item x");
            return false;
        }

        if (!QuestRequirement(trainingMethod.requirements.questIds))
        {
            EventManager.Instance.ShowPopUpMsg("Need quest x");
            return false;
        }

        if (!GeneralItemRequirement(trainingMethod.requirements.generalSkillItems))
        {
            EventManager.Instance.ShowPopUpMsg("Need some kind of x item");
            return false;
        }

        return true;
    }

    public void SkillButtonClicked(string skillName)
    {
        isTrainingMethodSelected = false;
        selectedSkill = skillsClasses[skillName];

        EventManager.Instance.SkillSelected(selectedSkill);
        EventManager.Instance.DrawTrainingMethods(selectedSkill.trainingMethods);
    }

    public void SetTrainingMethod(int index)
    {
        selectedTrainingMethod = selectedSkill.trainingMethods[index];

        if (CheckRequirement(selectedTrainingMethod))
        {
            isTrainingMethodSelected = true;
            actionCount = 0;
            dropTableDict = DropTableManager.CreateDropTableDictionary(selectedTrainingMethod.dropTables);
            EventManager.Instance.SkillingStarted();
        }
        else
        {
            isTrainingMethodSelected = false;
        }
    }

    private void OnTabClicked(int tabIndex)
    {
        isTrainingMethodSelected = false;
    }

    private void ClearDropTable(Dictionary<long, BigInteger> dropTable)
    {
        foreach (long id in dropTable.Keys.ToList())
        {
            dropTable[id] = 0;
        }
    }
}
