using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    #region Singleton
    private static EventManager _instance;
    public static EventManager Instance { get { return _instance; } }

    
    //TODO this is just silly
    public static void setIntance(EventManager inst)
    { 
        _instance = inst;
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    #endregion

    public event Action<string> onSkillClicked;
    public void SkillClicked(string skillName)
    {
        onSkillClicked?.Invoke(skillName);
    }

    public event Action<List<TrainingMethod>> onDrawTrainingMethods;
    public void DrawTrainingMethods(List<TrainingMethod> trainingMethods)
    {
        onDrawTrainingMethods?.Invoke(trainingMethods);
    }

    public event Action<int> onTrainingMethodClicked;
    public void TrainingMethodClicked(int index)
    {
        onTrainingMethodClicked?.Invoke(index);
    }

    public event Action<string, int, int> onLevelUp;
    public void LevelUp(string skillName, int lvl, int totalLvl)
    {
        onLevelUp?.Invoke(skillName, lvl, totalLvl);
    }

    public event Action<int> onXpGained;
    public void XpGained(int xp)
    {
        onXpGained?.Invoke(xp);
    }

    public event Action<long, int, int> onItemChanged;
    public void ItemChanged(long id, int amount, int slotInd)
    {
        onItemChanged?.Invoke(id, amount, slotInd);
    }

    //---- Tabs ---
    public event Action<int> onMainTabClicked;
    public void MainTabClicked(int tabInd)
    {
        onMainTabClicked?.Invoke(tabInd);
    }

    public event Action<int> onPlayerTabClicked;
    public void PlayerTabClicked(int tabInd)
    {
        onPlayerTabClicked?.Invoke(tabInd);
    }

}
