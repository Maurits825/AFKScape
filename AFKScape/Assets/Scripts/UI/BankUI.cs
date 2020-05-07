using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

public class BankUI : MonoBehaviour
{
    public GameObject slotPrefab;
    public Transform slotListParent;

    private Dictionary<long, Slot> slots = new Dictionary<long, Slot>();
    private Dictionary<long, Text> bankText = new Dictionary<long, Text>();
    private Dictionary<long, Image> bankImage = new Dictionary<long, Image>();

    void Start()
    {
        EventManager.Instance.OnBankItemAdded += BankItemAdded;
        EventManager.Instance.OnBankItemRemoved += BankItemRemoved;
        gameObject.SetActive(false);
    }

    void BankItemAdded(long id, BigInteger amount, BigInteger _)
    {
        if (amount > 0)
        {
            if (!bankText.ContainsKey(id))
            {
                //TODO put this into function or something
                GameObject slotObject = Instantiate(slotPrefab) as GameObject;
                slotObject.transform.SetParent(slotListParent, false);

                Slot slot = slotObject.GetComponent<Slot>();
                slot.SetItemName(Database.items[id].name);
                slots.Add(id, slot);

                bankText.Add(id, slot.amountText);
                bankImage.Add(id, slot.iconImage);
            }

            (bankText[id].text, bankText[id].color) = UtilityUI.FormatNumber(amount);
            bankImage[id].sprite = Resources.Load<Sprite>("Icons/" + id.ToString());
        }
    }

    public void BankItemRemoved(long id, BigInteger amount, BigInteger _)
    {
        if (bankText.ContainsKey(id))
        {
            bankText[id].text = amount.ToString();
            bankImage[id].sprite = Resources.Load<Sprite>("Icons/" + id.ToString());

            if (amount == 0)
            {
                slots[id].SetAlpha(0.6F);
            }
        }
    }
}
