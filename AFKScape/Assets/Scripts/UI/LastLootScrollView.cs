using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

public class LastLootScrollView : MonoBehaviour
{
    public GameObject slotPrefab;
    public Transform slotListParent;

    private List<GameObject> slotsObjects = new List<GameObject>();
    private Dictionary<long, BigInteger> lastLootAmount = new Dictionary<long, BigInteger>();
    private Dictionary<long, Text> lastLootText = new Dictionary<long, Text>();
    private Dictionary<long, Image> lastLootImage = new Dictionary<long, Image>();

    private void Start()
    {
        EventManager.Instance.OnBankItemAdded += UpdateLastLootUI;
        EventManager.Instance.OnBossClicked += ClearLastLootUI;
        EventManager.Instance.OnSkillButtonClicked += ClearLastLootUI;
    }

    private void UpdateLastLootUI(long id, BigInteger amount, BigInteger amountDiff)
    {
        if (!lastLootText.ContainsKey(id))
        {
            GameObject slotObject = Instantiate(slotPrefab);
            slotObject.transform.SetParent(slotListParent, false);
            slotsObjects.Add(slotObject);

            Slot slot = slotObject.GetComponent<Slot>();
            slot.SetItemName(Database.items[id].name);

            lastLootText.Add(id, slot.amountText);
            lastLootImage.Add(id, slot.iconImage);
            lastLootAmount.Add(id, 0);

            lastLootImage[id].sprite = Database.sprites[(int)id];
        }

        lastLootAmount[id] += amountDiff;
        (lastLootText[id].text, lastLootText[id].color) = UtilityUI.FormatNumber(lastLootAmount[id]);
    }

    //TODO when to call?
    //TODO should only be cleared when a "clear" button pressed?
    private void ClearLastLootUI(string bossName)
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
            lastLootAmount.Clear();
        }
    }
}
