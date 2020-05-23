using UnityEngine;
using UnityEngine.UI;

public class SkillsPageUI : MonoBehaviour
{
    public Text status;
    public Text currentXp;

    public GameObject trainingMethodPanel;
    public GameObject progressBar;
    public GameObject xpDropObj;
    public GameObject animationObj;

    private ProgressBar skillProgressbar;
    private XpDrop xpDrop;

    private int lastXp;

    private string skillIconName;

    // Start is called before the first frame update
    void Start()
    {
        skillProgressbar = progressBar.GetComponent<ProgressBar>();
        xpDrop = xpDropObj.GetComponent<XpDrop>();

        currentXp.text = "";
        status.text = "";

        animationObj.SetActive(false);

        EventManager.Instance.OnSkillSelected += SkillSelected;
        EventManager.Instance.OnXpGained += UpdateXp;
        EventManager.Instance.OnSkillingStarted += OnSkillingStarted;
        EventManager.Instance.OnLevelUp += OnLevelUp;
    }

    private void SkillSelected(Skill skill)
    {
        status.text = skill.skillName;

        trainingMethodPanel.SetActive(true);
        animationObj.SetActive(false);

        DrawProgressBar(skill);

        currentXp.text = skill.xp.ToString();
        lastXp = skill.xp;
    }

    private void DrawProgressBar(Skill skill)
    {
        skillIconName = "SkillIcons/" + skill.skillName + "_icon_large";
        skillProgressbar.SetIcon(skillIconName);
        int lvl = skill.currentLevel;
        skillProgressbar.InitProgressBar(Database.experienceTable[lvl - 1], Database.experienceTable[lvl]);
        skillProgressbar.UpdateProgressBar(skill.xp);
        progressBar.SetActive(true);
    }

    private void UpdateXp(int xp)
    {
        currentXp.text = xp.ToString();
        skillProgressbar.UpdateProgressBar(xp);
        xpDrop.StartXpDrop(skillIconName, xp - lastXp);

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
        animationObj.SetActive(true);
    }
}
