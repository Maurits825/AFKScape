using System.Collections.Generic;
using UnityEngine;

public class ItemLookup : MonoBehaviour
{
    public enum FilterType
    {
        None,
        Normal
    }

    public FilterType filterType;

    [ItemId]
    public List<long> idList = new List<long>();

    public List<(long, string, string)> GetItemId(string itemName)
    {
        List<(long, string, string)> itemList = new List<(long, string, string)>();
        foreach (KeyValuePair<long, Item> item in Database.items)
        {
            if (item.Value.name.IndexOf(itemName, System.StringComparison.OrdinalIgnoreCase) >= 0)
            {
                string extraInfo = "";
                if (item.Value.noted)
                {
                    extraInfo += " Noted";
                    if (filterType == FilterType.Normal)
                    {
                        continue;
                    }
                }
                if (item.Value.duplicate)
                {
                    extraInfo += " Duplicate";
                    if (filterType == FilterType.Normal)
                    {
                        continue;
                    }
                }
                if (item.Value.placeholder)
                {
                    extraInfo += " Placeholder";
                    if (filterType == FilterType.Normal)
                    {
                        continue;
                    }
                }

                itemList.Add((item.Key, item.Value.name, extraInfo));
            }
        }

        return itemList;
    }
}
