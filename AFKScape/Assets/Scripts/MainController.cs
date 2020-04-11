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

    //TODO add item databse, from json?
    //have one like this for only string to id, another for id to item?
    private static Dictionary<string, int> itemDataBase = new Dictionary<string, int>() { { "Shrimp", 1 } };

    //move these somewhere else?
    private readonly int speedUpConstant = 10;
    private float timeConstant;
    private float actionCount;

    // Start is called before the first frame update
    void Start()
    {
        timeConstant = (1.0F / (60.0F * 60.0F)) * speedUpConstant;

        Database.LoadAll();
        InitUI();

        inventory = new Inventory(inventorySlots);

        skillsClasses.Add("Fishing", new Fishing()); //these will need to be singleton classes, will basic skills need a special class?
        skillsClasses.Add("Woodcutting", new Woodcutting()); //some skills can have all the functionality included in skill class
    }

    // Update is called once per frame
    void Update()
    {
        if (selectedSkill != null)
        {
            MainGameLoop(selectedSkill.trainingMethods[0], selectedSkill);
        }
    }

    private int getLevel(int xp)
    {
        return Mathf.RoundToInt(xp / 10000.0F); //TODO xp table
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

    private void MainGameLoop(TrainingMethod trainingMethod, Skill skill)
    {
        //TODO UT this
        float actionIncrement = trainingMethod.baseResourceRate * Time.deltaTime * timeConstant;
        actionCount += actionIncrement;
        if (actionCount >= 1.0F)
        {
            int actions = (int)actionCount;
            skill.xpFloat += (trainingMethod.xpPerResource * actions);
            actionCount -= actions;

            RollResources(trainingMethod, skill, actions);


        }
        skill.currentLevel = getLevel(skill.xp);

        UILevelText[skill.name].text = string.Concat(skill.currentLevel, "/", skill.currentLevel);
        currentXp.text = skill.xp.ToString();
    }

    private void RollResources(TrainingMethod trainingMethod, Skill skill, int actions)
    {
        for (int roll = 0; roll < actions; roll++)
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

            long itemId;
            int amount;
            foreach (ClueDropTable clueTable in trainingMethod.dropTables.OfType<ClueDropTable>())
            {
                (itemId, amount) = clueTable.RollTable(skill.boostedLevel);
                if (itemId != -1)
                {
                    inventory.AddItem(itemId, amount);
                }
            }

            foreach (PetDropTable petTable in trainingMethod.dropTables.OfType<PetDropTable>())
            {
                (itemId, amount) = petTable.RollTable(skill.boostedLevel);
                if (itemId != -1)
                {
                    inventory.AddItem(itemId, amount);
                }
            }
        }
    }
}
