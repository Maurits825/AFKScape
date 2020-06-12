using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentUI : MonoBehaviour
{
    public List<Slot> equipmentSlots;

    void Start()
    {
        EventManager.Instance.OnItemEquipped += EquipItem;
    }

    private void EquipItem(long id, Equipment.EquipmentSlot equipmentSlot)
    {
        Slot slot = equipmentSlots[(int)equipmentSlot];

        slot.SetItemName(Database.items[id].name);
        slot.SetId(id);
        slot.SetState(Slot.State.Equipped);
        //slot.SetSlotActive(true);
    }
}
