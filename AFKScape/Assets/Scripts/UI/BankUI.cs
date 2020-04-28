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
    private List<Text> bankText = new List<Text>();
    private List<Image> bankImage = new List<Image>();
 
    void Start()
    {
        EventManager.Instance.onBankItemAdded += UpdateBankUI;
        gameObject.SetActive(false);
    }

    void UpdateBankUI(long id, int amount, int slotIndex)
    {
        if (slotIndex >= itemCount)
        {
            //TODO put this into function or something
            GameObject slotObject = Instantiate(slotPrefab) as GameObject;
            slotObject.transform.SetParent(slotListParent, false);

            Slot slot = slotObject.GetComponent<Slot>();
            slot.SetItemName(Database.items[id].name);
            
            bankText.Add(slot.amountText);
            bankImage.Add(slot.iconImage);

            itemCount++;
        }

        //TODO handle 0 amount, remove icon
        bankText[slotIndex].text = amount.ToString();
        bankImage[slotIndex].sprite = Resources.Load<Sprite>("Icons/" + id.ToString());
    }
}
