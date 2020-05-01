using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BankUI : MonoBehaviour
{
    [SerializeField]
    private GameObject slotPrefab;
    [SerializeField]
    private Transform slotListParent;

    private int itemCount = 0;
    private Dictionary<long, Text> bankText = new Dictionary<long, Text>();
    private Dictionary<long, Image> bankImage = new Dictionary<long, Image>();

    void Start()
    {
        EventManager.Instance.onBankItemAdded += UpdateBankUI;
        gameObject.SetActive(false);
    }

    void UpdateBankUI(long id, int amount)
    {
        if (!bankText.ContainsKey(id))
        {
            //TODO put this into function or something
            GameObject slotObject = Instantiate(slotPrefab) as GameObject;
            slotObject.transform.SetParent(slotListParent, false);

            Slot slot = slotObject.GetComponent<Slot>();
            slot.SetItemName(Database.items[id].name);
            
            bankText.Add(id, slot.amountText);
            bankImage.Add(id, slot.iconImage);

            itemCount++;
        }

        //TODO handle 0 amount, remove icon
        bankText[id].text = amount.ToString();
        bankImage[id].sprite = Resources.Load<Sprite>("Icons/" + id.ToString());
    }
}
