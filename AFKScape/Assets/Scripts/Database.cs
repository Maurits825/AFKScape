﻿using System.Collections.Generic;
using UnityEngine;

public static class Database
{
    public static string[] skillNames = new string[23];
    public static List<string> bossesNames = new List<string>();

    public static Dictionary<long, Item> items = new Dictionary<long, Item>();
    public static Dictionary<int, string> quest = new Dictionary<int, string>();

    public static List<int> experienceTable;

    private const int NumSpriteSheets = 7;
    public static List<Sprite> sprites = new List<Sprite>();

    public static List<string> cluesNames = new List<string>
    {
        "Clue scroll (Beginner)",
        "Clue scroll (Easy)",
        "Clue scroll (Medium)",
        "Clue scroll (Hard)",
        "Clue scroll (Elite)",
        "Clue scroll (Master)",
    };

    public static void LoadAll()
    {
        LoadSkills();
        LoadItems();
        LoadQuests();
        LoadExperienceTable();
        LoadBosses();
        LoadIcons();
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
        //TODO implement
    }

    public static void LoadBosses()
    {
        bossesNames = JsonHandler.GetBossesNames();
    }

    public static void LoadIcons()
    {
        for (int i = 1; i <= NumSpriteSheets; i++)
        {
            string spriteSheetPath = "Item Icons/spritesheet_" + i.ToString();
            Sprite[] spritesSheet = Resources.LoadAll<Sprite>(spriteSheetPath);
            sprites.AddRange(spritesSheet);
        }
    }
}
