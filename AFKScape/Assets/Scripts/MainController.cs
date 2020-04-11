using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class MainController : MonoBehaviour
{
    public GameObject UILevelTextParent;
    private Dictionary<string, Text> UILevelText = new Dictionary<string, Text>();

    private Dictionary<string, Skill> skillsClasses = new Dictionary<string, Skill>();
    private Skill selectedSkill;

    public Text status;
    public Text currentXp;

    private readonly int inventorySlots = 28;
    private static Inventory inventory;

    //move these somewhere else?
    private readonly int speedUpConstant = 10;
    private float timeConstant = (1.0F / (60.0F * 60.0F)) * 10;
    private float actionCount;

    // Start is called before the first frame update
    void Start()
    {
        Database.LoadAll();
        InitUI();
        InitStatic();

        skillsClasses.Add("Fishing", new Fishing()); //these will need to be singleton classes, will basic skills need a special class?
        skillsClasses.Add("Woodcutting", new Woodcutting()); //some skills can have all the functionality included in skill class
    }

    // Update is called once per frame
    void Update()
    {
        if (selectedSkill != null)
        {
            MainGameLoop(selectedSkill.trainingMethods[0], selectedSkill, Time.deltaTime);
            UpdateUI(selectedSkill);
        }
    }

    private int getLevel(int xp)
    {
        return (Mathf.RoundToInt(xp / 100.0F)) + 1; //TODO xp table
    }

    public void handleSkillbuttonClicked(Button button) //TODO uppercase, gonna messe up links, maybe to a list?
    {
        string skill = button.name;
        status.text = string.Concat("Selected Skill:\n", button.name);
        selectedSkill = skillsClasses[skill];
    }

    public void InitUI()
    {
        Text[] lvlTexts = UILevelTextParent.GetComponentsInChildren<Text>();
        foreach (Text lvlText in lvlTexts)
        {
            UILevelText.Add(lvlText.name, lvlText);
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

            RollResources(trainingMethod, skill);
            //remove consumables

            int newLvl = getLevel(skill.xp);

            if (newLvl != skill.currentLevel)
            {
                //update actionCount?
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
            }

        }
    }

    private void RollResources(TrainingMethod trainingMethod, Skill skill)
    {
        //fix oftype doesnt work when you load json file
        List<(long, int)> itemList;

        foreach (GeneralDropTable generalTable in trainingMethod.generalDropTable)
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

        long itemId;
        int amount;
        (itemId, amount) = trainingMethod.clueDropTable.RollTable(skill.boostedLevel);
        if (itemId != -1)
        {
            inventory.AddItem(itemId, amount);
        }

        (itemId, amount) = trainingMethod.petDropTable.RollTable(skill.boostedLevel);
        if (itemId != -1)
        {
            inventory.AddItem(itemId, amount);
        }
    }
    

    private void UpdateUI(Skill skill)
    {
        UILevelText[skill.name].text = string.Concat(skill.currentLevel, "/", skill.currentLevel);
        currentXp.text = skill.xp.ToString();
    }
}
