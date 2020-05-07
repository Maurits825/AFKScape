﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabGroup : MonoBehaviour
{
    public List<TabButton> tabButtons;
    public TabButton defaultButton;
    public Color tabIdle;
    public Color tabHover;
    public Color tabActive;

    public TabButton selectedTab;

    public List<GameObject> objectsToSwap;

    public void Start()
    {
        if (defaultButton != null)
        {
            OnTabSelected(defaultButton);
        }
    }

    public void Subscribe(TabButton button)
    {
        if (tabButtons == null)
        {
            tabButtons = new List<TabButton>();
        }
        tabButtons.Add(button);
    }

    public void OnTabEnter(TabButton button)
    {
        ResetTabs();
        if (selectedTab == null || button != selectedTab)
        {
            button.background.color = tabHover;
        }
    }

    public void OnTabExit(TabButton button)
    {
        ResetTabs();
    }

    public void OnTabSelected(TabButton button)
    {
        selectedTab = button;
        ResetTabs();
        button.background.color = tabActive;

        int index = button.transform.GetSiblingIndex();
        RaiseEvents(index);

        for (int i = 0; i < objectsToSwap.Count; i++)
        {
            if (i == index)
            {
                objectsToSwap[i].SetActive(true);
            }
            else
            {
                objectsToSwap[i].SetActive(false);
            }
        }
    }

    public virtual void RaiseEvents(int index)
    {

    }

    public void ResetTabs()
    {
        foreach (TabButton button in tabButtons)
        {
            if (selectedTab != null && button == selectedTab)
            {
                continue;
            }

            button.background.color = tabIdle;
        }
    }
}
