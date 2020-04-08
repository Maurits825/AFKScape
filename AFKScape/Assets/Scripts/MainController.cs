using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]
public struct JsonHelper
{
    public List<string> data;
}

public class MainController : MonoBehaviour
{
    public static string[] skillNames = new string[23];

    public GameObject UILevelTextParent;
    private Dictionary<string, Text> UILevelText = new Dictionary<string, Text>();

    private Dictionary<string, Skill> skillsClasses = new Dictionary<string, Skill>();
    private Skill selectedSkill;

    public Text status;
    public Text currentXp;

    // Start is called before the first frame update
    void Start()
    {
        LoadDataFromJson();

        skillsClasses.Add("Fishing", new Fishing()); //these will need to be singleton classes, will basic skills need a special class?
        skillsClasses.Add("Woodcutting", new Woodcutting()); //some skills can have all the functionality included in skill class

        Text[] lvlTexts = UILevelTextParent.GetComponentsInChildren<Text>();
        foreach (Text lvlText in lvlTexts)
        {
            UILevelText.Add(lvlText.name, lvlText);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (selectedSkill != null)
        {
            selectedSkill.xp += selectedSkill.trainingMethods[0].baseXpRate; //[0] index 0 for now
            selectedSkill.currentLevel = getLevel(selectedSkill.xp);

            UILevelText[selectedSkill.name].text = string.Concat(selectedSkill.currentLevel, "/", selectedSkill.currentLevel);
            currentXp.text = selectedSkill.xp.ToString();
        }
    }

    private int getLevel(int xp)
    {
        return Mathf.RoundToInt(xp / 10000.0F);
    }

    public void handleSkillbuttonClicked(Button button) //TODO uppercase, gonna messe up links, maybe to a list?
    {
        string skill = button.name;
        status.text = string.Concat("Selected Skill:\n", button.name);
        selectedSkill = skillsClasses[skill];
    }

    public static void LoadDataFromJson()
    {
        TextAsset skillJSON = Resources.Load<TextAsset>("JSON/Skills");
        JsonHelper jsonHelperSkills = JsonUtility.FromJson<JsonHelper>(skillJSON.text);
        skillNames = jsonHelperSkills.data.ToArray();

        //also load quest and stuff
    }
}
