using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillsPageUI : MonoBehaviour
{
    public Text status;
    public Text currentXp;

    public GameObject trainingMethodPanel;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.Instance.onSkillClicked += UpdateSkillSelected;
        EventManager.Instance.onXpGained += UpdateXp;
        EventManager.Instance.OnSkillingStarted += HideTrainingMethodPanel;
        EventManager.Instance.onSkillClicked += ShowTrainingMethodPanel;
    }

    private void UpdateSkillSelected(string skillName)
    {
        status.text = skillName;
    }

    private void UpdateXp(int xp)
    {
        currentXp.text = xp.ToString();
    }

    private void HideTrainingMethodPanel()
    {
        trainingMethodPanel.SetActive(false);
    }

    private void ShowTrainingMethodPanel(string _)
    {
        trainingMethodPanel.SetActive(true);
    }
}
