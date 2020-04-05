using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainController : MonoBehaviour
{
    private List<Skill> skillsClasses = new List<Skill>();
    private Skill selectedSkill;

    public Text fishingLvl;
    public Text woodcuttingLvl; //TODO make this a list?

    public Text fishingXp;
    public Text woodcuttingXp;

    public Text status;

    // Start is called before the first frame update
    void Start()
    {
        skillsClasses.Add(new Fishing()); //these will need to be singleton classes
        skillsClasses.Add(new Woodcutting());
    }

    // Update is called once per frame
    void Update()
    {
        if (selectedSkill != null)
        {
            selectedSkill.xp += selectedSkill.skillTrainingMethods[0].baseXpRate;
            int lvl = getLevel(selectedSkill.xp);
            selectedSkill.currentLevel = lvl;
        }

        //TODO how will we only update the required? does it matter?
        //fishingXp.text = skillsClasses[0].xp.ToString();
        //fishingLvl.text = string.Concat(skillsClasses[0].currentLevel, "/", skillsClasses[0].currentLevel);
        //woodcuttingXp.text = skillsClasses[1].xp.ToString();
        //woodcuttingLvl.text = string.Concat(skillsClasses[1].currentLevel, "/", skillsClasses[1].currentLevel);
    }

    private int getLevel(int xp)
    {
        return Mathf.RoundToInt(xp / 10000.0F);
    }

    public void handleSkillbuttonClicked(Button button)
    {
        status.text = string.Concat("Selected Skill:\n", button.name);
    }
}
