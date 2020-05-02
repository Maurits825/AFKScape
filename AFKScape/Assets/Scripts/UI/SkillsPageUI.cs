using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillsPageUI : MonoBehaviour
{
    public Text status;
    public Text currentXp;

    public GameObject trainingMethodPanel;
    public GameObject progressBar;
    private ProgressBar skillProgressbar;

    private int lastXp;

    // Start is called before the first frame update
    void Start()
    {
        skillProgressbar = progressBar.GetComponent<ProgressBar>();

        EventManager.Instance.OnSkillClicked += OnSkillClicked;
        EventManager.Instance.OnXpGained += UpdateXp;
        EventManager.Instance.OnSkillingStarted += OnSkillingStarted;
        EventManager.Instance.OnDrawProgressBar += DrawProgressBar;
        EventManager.Instance.OnLevelUp += OnLevelUp;
    }

    private void OnSkillClicked(string skillName)
    {
        status.text = skillName;

        trainingMethodPanel.SetActive(true);
    }

    private void DrawProgressBar(Skill skill)
    {
        skillProgressbar.SetIcon("SkillIcons/" + skill.skillName + "_icon_large");
        int lvl = skill.currentLevel;
        skillProgressbar.InitProgressBar(Database.experienceTable[lvl - 1], Database.experienceTable[lvl]);
        skillProgressbar.UpdateProgressBar(skill.xp);
        progressBar.SetActive(true);
    }

    private void UpdateXp(int xp)
    {
        currentXp.text = xp.ToString();
        skillProgressbar.UpdateProgressBar(xp);

        lastXp = xp;
    }

    private void OnLevelUp(string skillName, int lvl, int totalLvl)
    {
        skillProgressbar.InitProgressBar(Database.experienceTable[lvl - 1], Database.experienceTable[lvl]);
        skillProgressbar.UpdateProgressBar(lastXp);
    }

    private void OnSkillingStarted()
    {
        trainingMethodPanel.SetActive(false);
    }
}
