using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemLookup : MonoBehaviour
{
    public static List<(long, string, string)> GetItemId(string itemName)
    {
        List<(long, string, string)> itemList = new List<(long, string, string)>();
        foreach (KeyValuePair<long, Item> item in JsonHandler.items)
        {
            if (item.Value.name.IndexOf(itemName, System.StringComparison.OrdinalIgnoreCase) >= 0)
            {
                string extraInfo = "";
                if (item.Value.noted)
                {
                    extraInfo += " Noted";
                }
                if (item.Value.duplicate)
                {
                    extraInfo += " Duplicate";
                }
                if (item.Value.placeholder)
                {
                    extraInfo += " Placeholder";
                }

                itemList.Add((item.Key, item.Value.name, extraInfo));
            }
        }

        return itemList;
    }
}
