using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class MainController : MonoBehaviour
{
    public Dictionary<string, Skill> skillsClasses = new Dictionary<string, Skill>();
    private Skill selectedSkill;
    private int selectedTrainingMethodInd = 0;
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

        InitStatic();
        InitSkillsDict();

        //TODO foreach (skill in skillsClasses) EventManager.levelup ? for ui lvls and total lvl

        //TODO REMOVE!!
        OnSkillSelected("Fishing");
        SetTrainingMethod(0);
        MainGameLoop(skillsClasses["Fishing"].trainingMethods[0], skillsClasses["Fishing"], 20000);
    }

    // Update is called once per frame
    void Update()
    {
        if (selectedSkill != null && isTrainingMethodSelected)
        {
            MainGameLoop(selectedSkill.trainingMethods[selectedTrainingMethodInd], selectedSkill, Time.deltaTime);
        }
    }

    public static int getLevel(int xp)
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
        isTrainingMethodSelected = true;

        dropTableDict = DropTableManager.CreateDropTableDictionary(selectedSkill.trainingMethods[selectedTrainingMethodInd].dropTables);
    }

    public void InitStatic()
    {
        inventory = new Inventory(inventorySlots);
    }

    public void InitSkillsDict()
    {
        //combat training not included yet
        //these will need to be singleton classes
        skillsClasses.Add("Agility", new Agility());
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
                int newLvl = getLevel(skill.xp);
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
