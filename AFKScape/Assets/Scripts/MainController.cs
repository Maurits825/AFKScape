using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class MainController : MonoBehaviour
{
    public Dictionary<string, Skill> skillsClasses = new Dictionary<string, Skill>();
    private Skill selectedSkill;
    public int selectedTrainingMethodInd = 0;
    private bool isTrainingMethodSelected = false;

    private readonly int inventorySlots = 28;
    public static Inventory inventory;

    //move these somewhere else?
    //TODO init problems when uting
    public float timeConstant = (1.0F / (60.0F * 60.0F)) * 10;
    private float actionCount;
    private static int maxLvl = 126; //TODO move somewhere

    private Dictionary<long, int> dropTableDict = new Dictionary<long, int>();

    // Start is called before the first frame update
    void Start()
    {
        //TODO order of these? well skills now depends on database
        Database.LoadAll();

        EventManager.Instance.onSkillClicked += OnSkillSelected;
        EventManager.Instance.onTrainingMethodClicked += SetTrainingMethod;
        EventManager.Instance.onMainTabClicked += OnTabClicked;

        InitStatic();
        InitSkillClasses();
        EventManager.Instance.LevelUp("Fishing", 1, GetTotalLevel()); //TODO fix this to update total level at start
    }

    // Update is called once per frame
    void Update()
    {
        if (selectedSkill != null && isTrainingMethodSelected)
        {
            MainGameLoop(selectedSkill.trainingMethods[selectedTrainingMethodInd], selectedSkill, Time.deltaTime);
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
        skillsClasses.Add("Fishing", new Fishing()); //these will need to be singleton classes, will basic skills need a special class?
        skillsClasses.Add("Fletching", new Fletching());
        skillsClasses.Add("Herblore", new Herblore());
        skillsClasses.Add("Hunter", new Hunter());
        skillsClasses.Add("Mining", new Mining());
        skillsClasses.Add("Prayer", new Prayer());
        skillsClasses.Add("Runecraft", new Runecraft());
        skillsClasses.Add("Smithing", new Smithing());
        skillsClasses.Add("Thieving", new Thieving());
        skillsClasses.Add("Woodcutting", new Woodcutting()); //some skills can have all the functionality included in skill class
        //TODO foreach (skill in skillsClasses) EventManager.levelup ? for ui lvls and total lvl
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
        return maxLvl;
    }

    public bool LevelRequirement(List<LevelRequirement> Levelrequirement)
    {
        for (int i = 0; i < Levelrequirement.Count; i++)
        {
            string skillName = Levelrequirement[i].skillName;
            if (skillsClasses[skillName].boostedLevel < Levelrequirement[i].levelReq)
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
    }//TODO add general list check

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
        return true;
    }

    public bool CheckRequirement(TrainingMethod trainingMethod) //TODO implement quest requirements
    {
        if (!LevelRequirement(trainingMethod.requirements.levelRequirements))
        {
            return false; //requirement was not met and returns
        }
        if (!ItemRequirement(trainingMethod.requirements.itemIds))
        {
            return false;
        }
        if (!QuestRequirement(trainingMethod.requirements.questIds))
        {
            return false;
        }
        if (!GeneralItemRequirement(trainingMethod.requirements.generalSkillItems))
        {
            return false;
        }
        return true;
    }

    public void OnSkillSelected(string skillName)
    {
        isTrainingMethodSelected = false;
        selectedSkill = skillsClasses[skillName];

        EventManager.Instance.DrawTrainingMethods(selectedSkill.trainingMethods);
        EventManager.Instance.XpGained(selectedSkill.xp);
    }

    public void SetTrainingMethod(int i)
    {
        //TODO check reqs
        selectedTrainingMethodInd = i;

        if (CheckRequirement(selectedSkill.trainingMethods[selectedTrainingMethodInd]))
        {
            isTrainingMethodSelected = true;
            dropTableDict = DropTableManager.CreateDropTableDictionary(selectedSkill.trainingMethods[selectedTrainingMethodInd].dropTables);
        } else {
            isTrainingMethodSelected = false;
        }
    }

    private void OnTabClicked(int tabIndex)
    {
        isTrainingMethodSelected = false;
    }

    public void InitStatic()
    {
        inventory = new Inventory(inventorySlots);
    }

    public void MainGameLoop(TrainingMethod trainingMethod, Skill skill, float deltaTime)
    {
        float currentDeltaTime = deltaTime;
        float actionIncrement = skill.GetResourceRate(trainingMethod.baseResourceRate) * currentDeltaTime * timeConstant;
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
            //if (getlevel(xp) != skill.lvl) --> eventManager.levelup

            DropTableManager.RollResources(dropTableDict, trainingMethod, skill.boostedLevel);
            //TODO remove consumables

            if (skill.xp >= skill.xpNextLvl)
            {
                int newLvl = GetLevel(skill.xp);
                skill.xpNextLvl = Database.experienceTable[newLvl];
                float deltaTimePerAction = currentDeltaTime / actionIncrement;
                float timePassed = actionDone * deltaTimePerAction;
                float newDeltaTime = currentDeltaTime - timePassed;
                currentDeltaTime = newDeltaTime;
                actionDone = 0;

                skill.currentLevel = newLvl;
                if (skill.boostedLevel < skill.currentLevel)
                {
                    skill.boostedLevel = skill.currentLevel;
                }

                actionIncrement = skill.GetResourceRate(trainingMethod.baseResourceRate) * newDeltaTime * timeConstant;
                actionCount += actionIncrement;

                int totalLvl = GetTotalLevel();
                EventManager.Instance.LevelUp(skill.skillName, skill.currentLevel, totalLvl);
            }
        }

        AddItemsToInventory(dropTableDict);//TODO this will add to bank later
    }

    private void AddItemsToInventory(Dictionary<long, int> items)
    {
        foreach (long id in items.Keys.ToList())
        {
            inventory.AddItem(id, items[id]);
            items[id] = 0;
        }
    }

    public int GetTotalLevel()
    {
        int totalLvl = 0;
        foreach  (Skill skill in skillsClasses.Values)
        {
            totalLvl = totalLvl + skill.currentLevel;
        }

        return totalLvl;
    }

}
