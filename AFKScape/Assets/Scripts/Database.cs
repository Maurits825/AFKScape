using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class Database
{
    public static string[] skillNames = new string[23];

    public static Dictionary<long, Item> items = new Dictionary<long, Item>();
    public static Dictionary<int, string> quest = new Dictionary<int, string>();

    public static List<int> skillLevels;

    [Serializable]
    private struct JsonHelper
    {
        public List<string> data;
    }

    public static void LoadAll()
    {
        LoadSkills();
        LoadItems();
        LoadQuests();
        LoadLevels();
    }

    public static void LoadSkills()
    {
        TextAsset JsonString = Resources.Load<TextAsset>("JSON/Skills");
        JsonHelper jsonHelperSkills = JsonUtility.FromJson<JsonHelper>(JsonString.text);
        skillNames = jsonHelperSkills.data.ToArray();
    }

    public static void LoadItems()
    {
        TextAsset JsonString = Resources.Load<TextAsset>(string.Concat("JSON/", "Items"));
        ItemList itemList = JsonUtility.FromJson<ItemList>(JsonString.text);

        foreach (Item item in itemList.itemList)
        {
            items.Add(item.id, item);
        }
    }

    public static void LoadLevels()
    {
        skillLevels = JsonHandler.getSkillLevels();
    }

    public static void LoadQuests()
    {

    }

}
