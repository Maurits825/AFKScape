using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainController : MonoBehaviour
{
    public GameObject UILevelTextParent;
    private Dictionary<string, Text> UILevelText = new Dictionary<string, Text>();

    private Dictionary<string, Skill> skillsClasses = new Dictionary<string, Skill>();
    private Skill selectedSkill;

    public Text status;
    public Text currentXp;

    // Start is called before the first frame update
    void Start()
    {
        skillsClasses.Add("Fishing", new Fishing()); //these will need to be singleton classes
        skillsClasses.Add("Woodcutting", new Woodcutting());

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

    public void handleSkillbuttonClicked(Button button)
    {
        string skill = button.name;
        status.text = string.Concat("Selected Skill:\n", button.name);
        selectedSkill = skillsClasses[skill];
    }
}
