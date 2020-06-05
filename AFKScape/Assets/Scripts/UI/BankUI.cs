using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

public class BankUI : MonoBehaviour
{
    public GameObject slotPrefab;
    public Transform slotListParent;
    public Toggle bankButton;

    private Dictionary<long, Slot> slots = new Dictionary<long, Slot>();
    private Dictionary<long, Text> bankText = new Dictionary<long, Text>();
    private Dictionary<long, Image> bankImage = new Dictionary<long, Image>();

    private void Start()
    {
        EventManager.Instance.OnBankItemAdded += BankItemAdded;
        EventManager.Instance.OnBankItemRemoved += BankItemRemoved;
        bankButton.onValueChanged.AddListener(delegate { BankToggleValueChanged(bankButton); });
        gameObject.SetActive(false);
    }

    public void BankItemAdded(long id, BigInteger amount, BigInteger amoutDiff)
    {
        if (amount > 0)
        {
            if (!bankText.ContainsKey(id))
            {
                //TODO put this into function or something
                GameObject slotObject = Instantiate(slotPrefab);
                slotObject.transform.SetParent(slotListParent, false);

                Slot slot = slotObject.GetComponent<Slot>();
                slot.SetItemName(Database.items[id].name);
                slot.SetId(id);
                slot.SetState(Slot.State.Bank);
                slots.Add(id, slot);

                bankText.Add(id, slot.amountText);
                bankImage.Add(id, slot.iconImage);
                bankImage[id].sprite = Database.sprites[(int)id];
            }

            (bankText[id].text, bankText[id].color) = UtilityUI.FormatNumber(amount);
        }
    }

    public void BankItemRemoved(long id, BigInteger amount, BigInteger amoutDiff)
    {
        if (bankText.ContainsKey(id))
        {
            bankText[id].text = amount.ToString();

            if (amount == 0)
            {
                slots[id].SetAlpha(0.6F);
                bankImage[id].sprite = null; //TODO
            }
        }
    }
    
    private void BankToggleValueChanged(Toggle bankToggle)
    {
        EventManager.Instance.BankActiveChanged(bankToggle.isOn);
    }
}
