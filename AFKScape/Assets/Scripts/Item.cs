using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

[Serializable]
public class Item
{
    public long id;
    public string name;

    public bool tradeable;
    public bool tradeableOnGe;

    public bool stackable;
    public bool noted;
    public bool noteable;

    public bool duplicate;

    public long linkedIdItem;
    public long linkedIdNoted;

    public long linkedIdPlaceholder;
    public bool placeholder;

    public bool equipable;
    public bool equipableByPlayer;
    public bool equipableWeapon;

    public long cost;
    public long lowalch;
    public long highalch;
    public long buyLimit;

    public bool questItem;

    public Equipment equipment;
    public Weapon weapon;
}

[Serializable]
public class Equipment
{
    public int attack_stab;
    public int attack_slash;
    public int attack_crush;
    public int attack_magic;
    public int attack_ranged;

    public int defence_stab;
    public int defence_slash;
    public int defence_crush;
    public int defence_magic;
    public int defence_ranged;

    public int melee_strength;
    public int ranged_strength;
    public int magic_damage;

    public int prayer;

    public string slot;

    public Requirements requirements;
}

[Serializable]
public class Weapon
{
    public int attack_speed;
    public string weapon_type;
    public List<Stance> stances;
}

[Serializable]
public class Stance
{
    public string combat_style;
    public string attack_type;
    public string attack_style;
    public string experience;
    public object boosts;
}

[Serializable]
public class ItemList
{
    public List<Item> itemList;
}
