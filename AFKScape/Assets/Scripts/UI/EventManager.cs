using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager
{
    #region Singleton
    public static EventManager Instance { get; private set; }

    //TODO this is just silly
    public static void SetIntance(EventManager inst)
    { 
        Instance = inst;
    }
    #endregion

    //---- Skills ----
    public event Action<string> OnSkillButtonClicked;
    public void SkillButtonClicked(string skillName)
    {
        OnSkillButtonClicked?.Invoke(skillName);
    }

    public event Action<Skill> OnSkillSelected;
    public void SkillSelected(Skill skill)
    {
        OnSkillSelected?.Invoke(skill);
    }

    public event Action<List<TrainingMethod>> OnDrawTrainingMethods;
    public void DrawTrainingMethods(List<TrainingMethod> trainingMethods)
    {
        OnDrawTrainingMethods?.Invoke(trainingMethods);
    }

    public event Action<int> OnTrainingMethodClicked;
    public void TrainingMethodClicked(int index)
    {
        OnTrainingMethodClicked?.Invoke(index);
    }

    public event Action<string, int, int> OnLevelUp;
    public void LevelUp(string skillName, int lvl, int totalLvl)
    {
        OnLevelUp?.Invoke(skillName, lvl, totalLvl);
    }

    public event Action<int> OnXpGained;
    public void XpGained(int xp)
    {
        OnXpGained?.Invoke(xp);
    }

    public event Action OnSkillingStarted;
    public void SkillingStarted()
    {
        OnSkillingStarted?.Invoke();
    }

    //---- Bosses ----
    public event Action<string> OnBossClicked;
    public void BossClicked(string bossName)
    {
        OnBossClicked?.Invoke(bossName);
    }


    //---- Inventory ----
    public event Action<long, int> OnInvItemAdded;
    public void InvItemAdded(long id, int amount)
    {
        OnInvItemAdded?.Invoke(id, amount);
    }

    public event Action<long, int> OnInvItemRemoved;
    public void InvItemRemoved(long id, int amount)
    {
        OnInvItemRemoved?.Invoke(id, amount);
    }

    //---- Bank ----
    public event Action<long, int, int> OnBankItemAdded;
    public void BankItemAdded(long id, int amount, int amounDiff)
    {
        OnBankItemAdded?.Invoke(id, amount, amounDiff);
    }

    public event Action<long, int, int> OnBankItemRemoved;
    public void BankItemRemoved(long id, int amount, int amounDiff)
    {
        OnBankItemRemoved?.Invoke(id, amount, amounDiff);
    }

    //---- Tabs ----
    public event Action<int> OnMainTabClicked;
    public void MainTabClicked(int tabInd)
    {
        OnMainTabClicked?.Invoke(tabInd);
    }

    public event Action<int> OnPlayerTabClicked;
    public void PlayerTabClicked(int tabInd)
    {
        OnPlayerTabClicked?.Invoke(tabInd);
    }

    //---- Popup ----
    public event Action<string> OnShowPopUpMsg;
    public void ShowPopUpMsg(string msg)
    {
        OnShowPopUpMsg?.Invoke(msg);
    }
}
