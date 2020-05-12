using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;

public static class Database
{
    public static string[] skillNames = new string[23];
    public static List<string> bossesNames = new List<string>();

    public static Dictionary<long, Item> items = new Dictionary<long, Item>();
    public static Dictionary<int, string> quest = new Dictionary<int, string>();

    public static List<int> experienceTable;

    private const int numSpriteSheets = 7;
    public static Sprite mySpriteTest;
    public static List<Sprite> sprites = new List<Sprite>();


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

    public static IEnumerator LoadIcons()
    {
        for (int i = 0; i < numSpriteSheets; i++)
        {
            string spriteSheetPath = "Assets/Textures/Item Icons/spritesheet_" + i.ToString() + ".png";
            AsyncOperationHandle<IList<Sprite>> handle = Addressables.LoadAssetAsync<IList<Sprite>>(spriteSheetPath);
            yield return handle;
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                Debug.Log("Another done");
                sprites.AddRange(handle.Result);
                Addressables.Release(handle);
            }
        }
    }

    public static void LoadSpritesWhenReady(AsyncOperationHandle<IList<IList<Sprite>>> handleToCheck)
    {
        Debug.Log("evt called -- list");
        if (handleToCheck.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log("Succeeded -- list");
            //mySpriteTest = handleToCheck.Result;
        }
    }

    public static void LoadSpritesWhenReady(AsyncOperationHandle<IList<Sprite>> handleToCheck)
    {
        Debug.Log("evt called -- single");
        if (handleToCheck.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log("Succeeded -- single");
            sprites.AddRange(handleToCheck.Result);
        }
    }
}
