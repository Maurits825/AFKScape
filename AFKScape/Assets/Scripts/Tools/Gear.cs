using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{
    public Equipment equipment;
    public GameObject mainControllerObj;

    [Serializable]
    public class GearSet
    {
        public string name;
        [ItemId]
        public List<long> items = new List<long>();
    }

    [Serializable]
    public class GearList
    {
        public List<GearSet> gearSets = new List<GearSet>();
    }

    public GearList gearList;

    public void LoadGearList()
    {
        gearList = JsonHandler.LoadJsonFile<GearList>("JSON/GearList");
    }

    public void SaveGearList()
    {
        JsonHandler.SaveJsonFile<GearList>(gearList, "GearList");
    }

    public List<string> GetGearNames()
    {
        List<string> names = new List<string>();

        foreach (GearSet gearSet in gearList.gearSets)
        {
            names.Add(gearSet.name);
        }

        return names;
    }

    public void EquipItems(int index)
    {
        if (equipment == null)
        {
            equipment = mainControllerObj.GetComponent<MainController>().equipment;
        }

        foreach (long id in gearList.gearSets[index].items)
        {
            equipment.EquipItem(id);
        }
    }
}
