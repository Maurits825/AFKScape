﻿using System.Collections;
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

    void Start()
    {
        EventManager.Instance.OnBankItemAdded += UpdateLastLootUI;
        EventManager.Instance.OnBossClicked += ClearLastLootUI;
        EventManager.Instance.OnSkillButtonClicked += ClearLastLootUI;
    }

    void UpdateLastLootUI(long id, BigInteger _, BigInteger amountDiff)
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
            lastLootAmount.Add(id, 0);
        }

        lastLootAmount[id] += amountDiff;
        (lastLootText[id].text, lastLootText[id].color) = UtilityUI.FormatNumber(lastLootAmount[id]);
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
            lastLootAmount.Clear();
        }
    }
}
