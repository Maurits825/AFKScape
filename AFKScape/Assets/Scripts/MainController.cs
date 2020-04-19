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

    // Start is called before the first frame update
    void Start()
    {
        //TODO order of these?
        Database.LoadAll();

        EventManager.Instance.onSkillClicked += OnSkillSelected;
        EventManager.Instance.onTrainingMethodClicked += SetTrainingMethod;

        InitStatic();
        initSkillClasses();
    }

    // Update is called once per frame
    void Update()
    {
        if (selectedSkill != null && isTrainingMethodSelected)
        {
            MainGameLoop(selectedSkill.trainingMethods[selectedTrainingMethodInd], selectedSkill, Time.deltaTime);
        }
    }

    public void initSkillClasses()
    {
        //combat training not included
        skillsClasses.Add("Agility", new Agility());
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

    public static int getLevel(int xp)
    {
        for (int i = 0; i < Database.skillLevels.Count; i++)
        {
            if (xp < Database.skillLevels[i])
            {
                return i;
            }
        }
        return maxLvl;
    }

    public bool LvlRequirement(List<LevelRequirement> Levelrequirement)
    {
        for (int i = 0; i < Levelrequirement.Count; i++)
        {
            string skillName = Levelrequirement[i].skillName;
            Skill currentSkill = skillsClasses[skillName];
            if (currentSkill.boostedLevel < Levelrequirement[i].levelReq)
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

    public bool QuestRequirement(List<int> questIds)
    {
        return true;
    }

    public bool CheckRequirement(TrainingMethod requirement) //TODO implement quest requirements
    {
        if (!LvlRequirement(requirement.requirements.levelRequirements))
        {
            return false; //requirement was not met and returns
        }
        if (!ItemRequirement(requirement.requirements.itemIDs))
        {
            return false;
        }
        if (!QuestRequirement(requirement.requirements.questIDs))
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
    }

    public void SetTrainingMethod(int i)
    {
        //TODO check reqs
        selectedTrainingMethodInd = i;
        if (CheckRequirement(selectedSkill.trainingMethods[selectedTrainingMethodInd]))
        {
            isTrainingMethodSelected = true;
        } else {
            isTrainingMethodSelected = false;
        }
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

            RollResources(trainingMethod, skill);
            //TODO remove consumables

            int newLvl = getLevel(skill.xp);

            if (newLvl != skill.currentLevel)
            {
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
    }

    //TODO move this to drop table manager, this is droptable repsonsiblity to handle this
    //TODO have one entry point on DropTableManager: List<(long, int)> = trainingMethod.dropTableManager.roll()
    private void RollResources(TrainingMethod trainingMethod, Skill skill)
    {
        List<(long, int)> itemList;

        foreach (GeneralDropTable generalTable in trainingMethod.dropTables.OfType<GeneralDropTable>())
        {
            itemList = generalTable.RollTable();
            if (itemList.Count > 0)
            {
                foreach ((long, int) item in itemList)
                {
                    inventory.AddItem(item.Item1, item.Item2);
                }
            }
        }

        long itemId = -1;
        int amount = 0;
        foreach (ClueDropTable clueDropTable in trainingMethod.dropTables.OfType<ClueDropTable>())
        {
            (itemId, amount) = clueDropTable.RollTable(skill.boostedLevel);
        }

        if (itemId != -1)
        {
            inventory.AddItem(itemId, amount);
        }

        foreach (PetDropTable petDropTable in trainingMethod.dropTables.OfType<PetDropTable>())
        {
            (itemId, amount) = petDropTable.RollTable(skill.boostedLevel);
        }

        if (itemId != -1)
        {
            inventory.AddItem(itemId, amount);
        }
    }

    private int GetTotalLevel()
    {
        int totalLvl = 0;
        foreach  (Skill skill in skillsClasses.Values)
        {
            totalLvl = totalLvl + skill.currentLevel;
        }

        return totalLvl;
    }

}
