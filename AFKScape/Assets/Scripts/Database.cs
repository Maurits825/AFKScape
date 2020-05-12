using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using System.Text.RegularExpressions;

public static class Database
{
    public static string[] skillNames = new string[23];
    public static List<string> bossesNames = new List<string>();

    public static Dictionary<long, Item> items = new Dictionary<long, Item>();
    public static Dictionary<int, string> quest = new Dictionary<int, string>();

    public static List<int> experienceTable;

    private const int numSpriteSheets = 7;
    private static int sheetsLoaded = 0;
    //TODO check if this duplicates the sprite in memory, shouldnt?
    private static IList<Sprite>[] loadedSprites = new IList<Sprite>[numSpriteSheets];
    public static List<Sprite> sprites = new List<Sprite>();


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

    }

    public static void LoadBosses()
    {
        bossesNames = JsonHandler.GetBossesNames();
    }

    public static void LoadIcons()
    {
        for (int i = 1; i <= numSpriteSheets; i++)
        {
            string spriteSheetPath = "Assets/Textures/Item Icons/spritesheet_" + i.ToString() + ".png";
            AsyncOperationHandle<IList<Sprite>> spriteHandle = Addressables.LoadAssetAsync<IList<Sprite>>(spriteSheetPath);
            spriteHandle.Completed += SpriteSheetLoaded;
        }
    }

    private static void SpriteSheetLoaded(AsyncOperationHandle<IList<Sprite>> handleToCheck)
    {
        if (handleToCheck.Status == AsyncOperationStatus.Succeeded)
        {
            int index = Int32.Parse(Regex.Match(handleToCheck.Result[0].texture.name, @"\d+").Value);
            loadedSprites[index - 1] = handleToCheck.Result;
            sheetsLoaded++;

            if (sheetsLoaded == numSpriteSheets)
            {
                LoadSpriteSheets();
            }
        }
    }

    private static void LoadSpriteSheets()
    {
        for (int i = 0; i < numSpriteSheets; i++)
        {
            sprites.AddRange(loadedSprites[i]);
        }
        Debug.Log("Sprites Loaded!");
    }
}
