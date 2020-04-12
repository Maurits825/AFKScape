using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class MainController : MonoBehaviour
{
    //move to ui script?
    public GameObject UILevelTextParent;
    private Dictionary<string, Text> UILevelText = new Dictionary<string, Text>();

    //TODO is there a better way
    public GameObject TrainingMethodPanel;
    private TrainingMethodScrollView trainingMethodScrollView;

    public GameObject inventoryPanel;
    private Dictionary<int, Text> inventoryText = new Dictionary<int, Text>();

    private Dictionary<string, Skill> skillsClasses = new Dictionary<string, Skill>();
    private Skill selectedSkill;
    private int selectedTrainingMethodInd = 0;
    private bool isTrainingMethodSelected = false;

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

        trainingMethodScrollView = TrainingMethodPanel.GetComponent<TrainingMethodScrollView>();

        InitUI();
        InitStatic();

        skillsClasses.Add("Fishing", new Fishing()); //these will need to be singleton classes, will basic skills need a special class?
        skillsClasses.Add("Woodcutting", new Woodcutting()); //some skills can have all the functionality included in skill class

        UpdateTotalLevel();
    }

    // Update is called once per frame
    void Update()
    {
        if (selectedSkill != null && isTrainingMethodSelected)
        {
            MainGameLoop(selectedSkill.trainingMethods[selectedTrainingMethodInd], selectedSkill, Time.deltaTime);
            UpdateUI(selectedSkill);
            UpdateInventoryUI();
        }
    }

    private int getLevel(int xp)
    {
        return (Mathf.RoundToInt(xp / 100.0F)) + 1; //TODO xp table
    }

    public void handleSkillbuttonClicked(Button button) //TODO uppercase, gonna messe up links, maybe to a list?
    {
        isTrainingMethodSelected = false;
        string skill = button.name;
        status.text = skill;
        selectedSkill = skillsClasses[skill];

        trainingMethodScrollView.CreateTrainingMethodButtons(selectedSkill.trainingMethods);
    }

    public void SetTrainingMethod(int i)
    {
        selectedTrainingMethodInd = i;
        isTrainingMethodSelected = true;
    }

    public void InitUI()
    {
        Text[] lvlTexts = UILevelTextParent.GetComponentsInChildren<Text>();
        foreach (Text lvlText in lvlTexts)
        {
            UILevelText.Add(lvlText.name, lvlText);
        }

        Text[] invTexts = inventoryPanel.GetComponentsInChildren<Text>();
        int slot = 0;
        foreach (Text invText in invTexts)
        {
            inventoryText.Add(slot, invText);
            slot++;
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
            }

        }
    }

    private void RollResources(TrainingMethod trainingMethod, Skill skill)
    {
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
    private int GetTotalLevel()
    {
        int totalLvl = 0;
        foreach  (KeyValuePair<string, Skill> skill in skillsClasses)
        {
            totalLvl = totalLvl + skill.Value.currentLevel;
        }

        return totalLvl;
    }

    private void UpdateInventoryUI()
    {
        int index = 0;
        foreach (KeyValuePair<long, int> item in inventory.items)
        {
            inventoryText[index].text = string.Concat(Database.items[item.Key].name, "\n", item.Value.ToString());
        }
    }
    private void UpdateUI(Skill skill)
    {
        UILevelText[skill.name].text = string.Concat(skill.currentLevel, "/", skill.currentLevel, "  "); //two space for formating...
        UpdateTotalLevel();
        currentXp.text = skill.xp.ToString();
    }

    private void UpdateTotalLevel()
    {
        UILevelText["TotalLvl"].text = string.Concat("Total level:\n", GetTotalLevel().ToString());
    }
}
