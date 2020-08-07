using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentUI : MonoBehaviour
{
    public List<Slot> equipmentSlots;
    public GameObject statsPanel;

    private EquipmentStats equipmentStats = new EquipmentStats();
    private bool isStatsPanelActive = false;

    void Start()
    {
        EventManager.Instance.OnItemEquipped += EquipItem;
        EventManager.Instance.OnItemUnEquipped += UnEquipItem;
        EventManager.Instance.OnUpdateTotalEquipmentStats += UpdateTotalEquipmentStats;

        statsPanel.SetActive(false);
    }

    //TODO clean up maybe?
    public void ShowStats()
    {
        if (isStatsPanelActive)
        {
            statsPanel.SetActive(false);
            isStatsPanelActive = false;
        }
        else
        {
            statsPanel.SetActive(true);
            isStatsPanelActive = true;
            Text statsText = statsPanel.GetComponentInChildren<Text>();
            string txt = "attackStab: " + equipmentStats.attackStab.ToString()
            +"\nattackSlash: " + equipmentStats.attackSlash.ToString()
            +"\nattackCrush: " + equipmentStats.attackCrush.ToString()
            +"\nattackMagic: " + equipmentStats.attackMagic.ToString()
            +"\nattackRanged: " + equipmentStats.attackRanged.ToString()

            +"\ndefenceStab: " + equipmentStats.defenceStab.ToString()
            +"\ndefenceSlash: " + equipmentStats.defenceSlash.ToString()
            +"\ndefenceCrush: " + equipmentStats.defenceCrush.ToString()
            +"\ndefenceMagic: " + equipmentStats.defenceMagic.ToString()
            +"\ndefenceRanged: " + equipmentStats.defenceRanged.ToString()

            +"\nmeleeStrength: " + equipmentStats.meleeStrength.ToString()
            +"\nrangedStrength: " + equipmentStats.rangedStrength.ToString()
            +"\nmagicDamage: " + equipmentStats.magicDamage.ToString()

            +"\nprayer: " + equipmentStats.prayer.ToString();

            statsText.text = txt;
        }
    }

    private void EquipItem(long id, Equipment.EquipmentSlot equipmentSlot, BigInteger amount)
    {
        Slot slot = equipmentSlots[(int)equipmentSlot];

        slot.SetItemName(Database.items[id].name);
        slot.SetId(id);
        slot.SetState(Slot.State.Equipped);
        slot.iconImage.sprite = Database.sprites[(int)id];
        (slot.amountText.text, slot.amountText.color) = UtilityUI.FormatNumber(amount);

        slot.SetSlotActive(true);
    }

    private void UnEquipItem(long id, Equipment.EquipmentSlot equipmentSlot)
    {
        Slot slot = equipmentSlots[(int)equipmentSlot];
        slot.SetSlotActive(false);
    }

    private void UpdateTotalEquipmentStats(EquipmentStats stats)
    {
        equipmentStats = stats;
    }
}
