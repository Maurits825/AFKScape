using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

[Serializable]
public class Item
{
    public Info info;

    [Serializable]
    public class Info
    {
        public long id;
        public string name;
        public bool Incomplete { get; set; }
        public bool Members { get; set; }
        public bool Tradeable { get; set; }

        public bool TradeableOnGe { get; set; }

        public bool Stackable { get; set; }

        public bool Noted { get; set; }

        public bool Noteable { get; set; }

        public object LinkedIdItem { get; set; }

        public long LinkedIdNoted { get; set; }

        public long LinkedIdPlaceholder { get; set; }

        public bool Placeholder { get; set; }

        public bool Equipable { get; set; }

        public bool EquipableByPlayer { get; set; }

        public bool EquipableWeapon { get; set; }

        public long cost;

        public long Lowalch { get; set; }

        public long Highalch { get; set; }

        public double Weight { get; set; }

        public long BuyLimit { get; set; }

        public bool QuestItem { get; set; }

        public DateTimeOffset ReleaseDate { get; set; }

        public bool Duplicate { get; set; }

        public string Examine { get; set; }

        public string WikiName { get; set; }

        public Uri WikiUrl { get; set; }
        // public Equipment Equipment { get; set; }

        //public Weapon Weapon { get; set; }

        public Info()
        {
            id = 0;
            name = "test";
            cost = 3;
        }
    }
}