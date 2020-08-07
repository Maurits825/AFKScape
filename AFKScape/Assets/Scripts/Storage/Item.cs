using System;
using System.Collections.Generic;

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

    public EquipmentStats equipment;
    public Weapon weapon;
}

[Serializable]
public class EquipmentStats
{
    public int attackStab;
    public int attackSlash;
    public int attackCrush;
    public int attackMagic;
    public int attackRanged;

    public int defenceStab;
    public int defenceSlash;
    public int defenceCrush;
    public int defenceMagic;
    public int defenceRanged;

    public int meleeStrength;
    public int rangedStrength;
    public int magicDamage;

    public int prayer;

    public Equipment.EquipmentSlot slot;

    public Requirements requirements;
}

[Serializable]
public class Weapon
{
    public int attackSpeed;
    public string weaponType;
    public List<Stance> stances;
}

[Serializable]
public class Stance
{
    public string combatStyle;
    public string attackType;
    public string attackStyle;
    public string experience;
    public object boosts;
}

[Serializable]
public class ItemList
{
    public List<Item> itemList;
}
