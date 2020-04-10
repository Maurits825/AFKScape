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
        public bool Iincomplete;
        public bool members;
        public bool tradeable;

        public bool tradeableOnGe;

        public bool stackable;

        public bool noted;

        public bool noteable;

        public object linkedIdItem;

        public long linkedIdNoted;

        public long linkedIdPlaceholder;

        public bool placeholder;

        public bool equipable;

        public bool equipableByPlayer;

        public bool equipableWeapon;

        public long cost;

        public long lowalch;

        public long highalch;

        public double weight;

        public long buyLimit;

        public bool questItem;

        public DateTimeOffset releaseDate;

        public bool duplicate;

        public string examine;

        public string wwikiName;

        public Uri wikiUrl;
        // public Equipment Equipment { get; set; }

        //public Weapon Weapon { get; set; }
    }
}

[Serializable]
public class ItemList
{
    public List<Item> itemList;
}