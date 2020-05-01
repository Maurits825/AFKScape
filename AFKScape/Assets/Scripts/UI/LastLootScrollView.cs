using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LastLootScrollView : MonoBehaviour
{
    [SerializeField]
    private GameObject slotPrefab;
    [SerializeField]
    private Transform slotListParent;

    private List<GameObject> slotsObjects = new List<GameObject>();
    private Dictionary<long, Text> lastLootText = new Dictionary<long, Text>();
    private Dictionary<long, Image> lastLootImage = new Dictionary<long, Image>();

    void Start()
    {
        EventManager.Instance.onBankItemAdded += UpdateLastLootUI;
        EventManager.Instance.onBossClicked += ClearLastLootUI;
        EventManager.Instance.onSkillClicked += ClearLastLootUI;
    }

    void UpdateLastLootUI(long id, int amount)
    {
        if (!lastLootText.ContainsKey(id))
        {
            GameObject slotObject = Instantiate(slotPrefab) as GameObject;
            slotObject.transform.SetParent(slotListParent, false);
            slotsObjects.Add(slotObject);

            Slot slot = slotObject.GetComponent<Slot>();
            slot.SetItemName(Database.items[id].name);

            lastLootText.Add(id, slot.amountText);
            lastLootImage.Add(id, slot.iconImage);
        }

        lastLootText[id].text = amount.ToString();
        lastLootImage[id].sprite = Resources.Load<Sprite>("Icons/" + id.ToString());
    }

    //TODO when to call?
    //TODO should only be cleared when a "clear" button pressed?
    void ClearLastLootUI(string _)
    {
        if (slotsObjects.Count > 0)
        {
            foreach (GameObject slot in slotsObjects)
            {
                Destroy(slot);
            }

            slotsObjects.Clear();
            lastLootText.Clear();
            lastLootImage.Clear();
        }
    }
}
