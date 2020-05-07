﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class Database
{
    public static string[] skillNames = new string[23];
    public static List<string> bossesNames = new List<string>();

    public static Dictionary<long, Item> items = new Dictionary<long, Item>();
    public static Dictionary<int, string> quest = new Dictionary<int, string>();

    public static List<int> experienceTable;

    public static void LoadAll()
    {
        LoadSkills();
        LoadItems();
        LoadQuests();
        LoadExperienceTable();
        LoadBosses();
    }

    public static void LoadSkills()
    {
        skillNames = JsonHandler.GetLoadedSkills();
    }

    public static void LoadItems()
    {
        ItemList itemList = JsonHandler.GetLoadedItems();

        foreach (Item item in itemList.itemList)
        {
            items.Add(item.id, item);
        }
    }

    public static void LoadExperienceTable()
    {
        experienceTable = JsonHandler.GetSkillLevels();
    }

    public static void LoadQuests()
    {

    }

    public static void LoadBosses()
    {
        bossesNames = JsonHandler.GetBossesNames();
    }

}
