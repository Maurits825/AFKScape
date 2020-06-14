using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentUI : MonoBehaviour
{
    public List<Slot> equipmentSlots;

    void Start()
    {
        EventManager.Instance.OnItemEquipped += EquipItem;
        EventManager.Instance.OnItemUnEquipped += UnEquipItem;
    }

    private void EquipItem(long id, Equipment.EquipmentSlot equipmentSlot)
    {
        Slot slot = equipmentSlots[(int)equipmentSlot];

        slot.SetItemName(Database.items[id].name);
        slot.SetId(id);
        slot.SetState(Slot.State.Equipped);
        slot.iconImage.sprite = Database.sprites[(int)id];
        slot.SetSlotActive(true);
    }

    private void UnEquipItem(long id, Equipment.EquipmentSlot equipmentSlot)
    {
        Slot slot = equipmentSlots[(int)equipmentSlot];
        slot.SetSlotActive(false);
    }
}
