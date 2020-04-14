using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
    public GameObject levelTextParent;
    public GameObject levelButtonParent;
    private Dictionary<string, Text> levelTexts = new Dictionary<string, Text>();
    private Dictionary<string, Text> levelButtons = new Dictionary<string, Text>();

    //TODO
    public Text status;
    public Text currentXp;

    public void InitUI()
    {
        Text[] lvlTexts = levelTextParent.GetComponentsInChildren<Text>();
        foreach (Text lvlText in lvlTexts)
        {
            levelTexts.Add(lvlText.name, lvlText);
        }

        Button[] lvlButtons = levelButtonParent.GetComponentsInChildren<Button>();
        foreach (Button button in lvlButtons)
        {
            button.onClick.AddListener(() => EventManager.Instance.SkillClicked(button.name));
        }
    }

    private void UpdateUISkill(string skillName, int lvl, int totalLvl)
    {
        levelTexts[skillName].text = string.Concat(lvl, "/", lvl, " "); //space for formating...
        levelTexts["TotalLvl"].text = string.Concat("Total level:\n", totalLvl);
    }

    private void UpdateSkillSelected(string skillName)
    {
        status.text = skillName;
    }

    private void UpdateXp(int xp)
    {
        currentXp.text = xp.ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        EventManager.Instance.onLevelUp += UpdateUISkill;
        EventManager.Instance.onSkillClicked += UpdateSkillSelected;
        EventManager.Instance.onXpGained += UpdateXp;
        InitUI();
    }
}
