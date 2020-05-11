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

    public static Sprite mySpriteTest;
    public static int idCount = 0;

    public static void LoadAll()
    {
        LoadSkills();
        LoadItems();
        LoadQuests();
        LoadExperienceTable();
        LoadBosses();
        //LoadIcons();
        //LoadIcon();
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
        Debug.Log("creating icons list");

        List<string[]> itemPath = new List<string[]>();

        List<string> currentPath = new List<string>();
        int total = 10000;
        for (int i = 0; i < total; i++)
        {
            if (((i % 1000) == 0) && (i != 0))
            {
                itemPath.Add(currentPath.ToArray());
                currentPath.Clear();
            }
            currentPath.Add("Assets/Textures/Icons/" + i.ToString() + ".png");
        }

        Debug.Log("Start to load");
        for (int i = 0; i < 1; i++)
        {
            Addressables.LoadAssetsAsync<Sprite>(itemPath[i], OnSpriteLoaded, Addressables.MergeMode.Union).Completed += LoadSpritesWhenReady;
        }
        //AsyncOperationHandle<IList<Sprite>> spriteHandle = Addressables.LoadAssetsAsync<Sprite>(itemPath[0], OnSpriteLoaded, Addressables.MergeMode.Union);
        //spriteHandle.Completed += LoadSpritesWhenReady;
    }

    public static void LoadIcon()
    {
        AsyncOperationHandle<Sprite> spriteHandle = Addressables.LoadAssetAsync<Sprite>("Assets/Textures/Icons/0.png");
        spriteHandle.Completed += LoadSpritesWhenReady;
    }

    private static void OnSpriteLoaded(Sprite sprite)
    {
        idCount++;
        if ((idCount % 100) == 0)
        {
            Debug.Log(idCount);
        }
    }

    public static void LoadSpritesWhenReady(AsyncOperationHandle<IList<Sprite>> handleToCheck)
    {
        Debug.Log("evt called -- list");
        if (handleToCheck.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log("Succeeded -- list");
            //mySpriteTest = handleToCheck.Result;
        }
    }

    public static void LoadSpritesWhenReady(AsyncOperationHandle<Sprite> handleToCheck)
    {
        Debug.Log("evt called -- single");
        if (handleToCheck.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log("Succeeded -- single");
            mySpriteTest = handleToCheck.Result;
        }
    }
}
