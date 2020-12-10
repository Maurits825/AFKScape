using System.Numerics;

public class Equipment
{
    private Inventory inventory;

    public enum EquipmentSlot
    {
        head = 0,
        cape,
        neck,
        ammo,
        weapon,
        body,
        shield,
        legs,
        hands,
        feet,
        ring,
        twoHanded,
    }

    private long[] equipedItems = new long[11];
    private bool isTwoHandedEquipped = false;
    private BigInteger ammoCount = 0;
    private BigInteger prevAmmoCount = 0;

    private EquipmentStats totalEquipmentStats = new EquipmentStats();

    public Equipment(Inventory inventory)
    {
        Initialize(inventory);
    }

    private void Initialize(Inventory inventory)
    {
        this.inventory = inventory;
    }

    //TODO requirments check
    public void EquipItem(long id)
    {
        if (Database.items[id].equipableByPlayer || Database.items[Database.items[id].linkedIdItem].equipableByPlayer)
        {
            EquipmentSlot slot;
            if (Database.items[id].equipableByPlayer)
            {
                slot = Database.items[id].equipment.slot;
            }
            else
            {
                slot = Database.items[Database.items[id].linkedIdItem].equipment.slot;
            }

            int slotIndex;
            EquipmentSlot actualSlot;
            if (slot == EquipmentSlot.twoHanded)
            {
                slotIndex = (int)EquipmentSlot.weapon;
                actualSlot = EquipmentSlot.weapon;
            }
            else
            {
                slotIndex = (int)slot;
                actualSlot = slot;
            }

            BigInteger amount;
            if (slot == EquipmentSlot.ammo)
            {
                amount = inventory.RemoveItem(id, inventory.GetAmount(id));
                prevAmmoCount = ammoCount;
                ammoCount = amount;
            }
            else
            {
                amount = inventory.RemoveItem(id, 1);
            }

            if (slot == EquipmentSlot.twoHanded)
            {
                if (equipedItems[(int)EquipmentSlot.weapon] != 0)
                {
                    UnEquipItem(equipedItems[(int)EquipmentSlot.weapon]);
                }

                if (equipedItems[(int)EquipmentSlot.shield] != 0)
                {
                    UnEquipItem(equipedItems[(int)EquipmentSlot.shield]);
                }

                isTwoHandedEquipped = true;
            }
            else if (slot == EquipmentSlot.shield)
            {
                if (equipedItems[(int)EquipmentSlot.shield] != 0)
                {
                    UnEquipItem(equipedItems[(int)EquipmentSlot.shield]);
                }
                else if (isTwoHandedEquipped)
                {
                    UnEquipItem(equipedItems[(int)EquipmentSlot.weapon]);
                }

                isTwoHandedEquipped = false;
            }
            else if (slot == EquipmentSlot.weapon)
            {
                if (equipedItems[(int)EquipmentSlot.weapon] != 0)
                {
                    UnEquipItem(equipedItems[(int)EquipmentSlot.weapon]);
                }

                isTwoHandedEquipped = false;
            }
            else if (equipedItems[slotIndex] != 0)
            {
                UnEquipItem(equipedItems[slotIndex]);
            }

            equipedItems[slotIndex] = id;
            EventManager.Instance.ItemEquipped(id, actualSlot, amount);
            UpdateTotalEquipmentStats();
        }
    }

    public void UnEquipItem(long id)
    {
        EquipmentSlot slot;
        if (Database.items[id].equipableByPlayer)
        {
            slot = Database.items[id].equipment.slot;
        }
        else
        {
            slot = Database.items[Database.items[id].linkedIdItem].equipment.slot;
        }

        int slotIndex;
        if (slot == EquipmentSlot.twoHanded)
        {
            slotIndex = (int)EquipmentSlot.weapon;
        }
        else
        {
            slotIndex = (int)slot;
        }

        equipedItems[slotIndex] = 0;

        if (slot == EquipmentSlot.ammo)
        {
            inventory.AddItem(id, prevAmmoCount);
            prevAmmoCount = ammoCount;
        }
        else
        {
            inventory.AddItem(id, 1);
        }

        if (slot == EquipmentSlot.twoHanded)
        {
            slot = EquipmentSlot.weapon;
        }

        EventManager.Instance.ItemUnEquipped(id, slot);
        UpdateTotalEquipmentStats();
    }

    public EquipmentStats GetTotalEquipmentStats()
    {
        return totalEquipmentStats;
    }

    private void UpdateTotalEquipmentStats()
    {
        totalEquipmentStats.attackStab = 0;
        totalEquipmentStats.attackSlash = 0;
        totalEquipmentStats.attackCrush = 0;
        totalEquipmentStats.attackMagic = 0;
        totalEquipmentStats.attackRanged = 0;

        totalEquipmentStats.defenceStab = 0;
        totalEquipmentStats.defenceSlash = 0;
        totalEquipmentStats.defenceCrush = 0;
        totalEquipmentStats.defenceMagic = 0;
        totalEquipmentStats.defenceRanged = 0;

        totalEquipmentStats.meleeStrength = 0;
        totalEquipmentStats.rangedStrength = 0;
        totalEquipmentStats.magicDamage = 0;

        totalEquipmentStats.prayer = 0;

        foreach (long id in equipedItems)
        {
            totalEquipmentStats.attackStab += Database.items[id].equipment.attackStab;
            totalEquipmentStats.attackSlash += Database.items[id].equipment.attackSlash;
            totalEquipmentStats.attackCrush += Database.items[id].equipment.attackCrush;
            totalEquipmentStats.attackMagic += Database.items[id].equipment.attackMagic;
            totalEquipmentStats.attackRanged += Database.items[id].equipment.attackRanged;

            totalEquipmentStats.defenceStab += Database.items[id].equipment.defenceStab;
            totalEquipmentStats.defenceSlash += Database.items[id].equipment.defenceSlash;
            totalEquipmentStats.defenceCrush += Database.items[id].equipment.defenceCrush;
            totalEquipmentStats.defenceMagic += Database.items[id].equipment.defenceMagic;
            totalEquipmentStats.defenceRanged += Database.items[id].equipment.defenceRanged;

            totalEquipmentStats.meleeStrength += Database.items[id].equipment.meleeStrength;
            totalEquipmentStats.rangedStrength += Database.items[id].equipment.rangedStrength;
            totalEquipmentStats.magicDamage += Database.items[id].equipment.magicDamage;

            totalEquipmentStats.prayer += Database.items[id].equipment.prayer;
        }

        EventManager.Instance.UpdateTotalEquipmentStats(totalEquipmentStats);
    }
}
