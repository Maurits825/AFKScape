using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemLookup : MonoBehaviour
{
    public static long GetItemId(string itemName)
    {
        foreach (KeyValuePair<long, Item> item in Database.items)
        {
            if (string.Equals(item.Value.name, itemName, System.StringComparison.OrdinalIgnoreCase))
            {
                return item.Key;
            }
        }
        return -1;
    }
}
