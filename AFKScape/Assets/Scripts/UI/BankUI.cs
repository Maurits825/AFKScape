using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BankUI : MonoBehaviour
{
    [SerializeField]
    private GameObject slotPrefab;

    private int itemCount = 0;
    private List<Text> bankText = new List<Text>();
    private List<Image> bankImage = new List<Image>();
 
    void Start()
    {
        EventManager.Instance.onBankItemAdded += UpdateBankUI;
    }

    void UpdateBankUI(long id, int amount, int slotIndex)
    {
        if (slotIndex >= itemCount)
        {
            //item is not in the bank, create new icon
            GameObject slot = Instantiate(slotPrefab) as GameObject;
            slot.transform.SetParent(transform, false);
            bankText.Add(slot.GetComponentInChildren<Text>());
            bankImage.Add(slot.GetComponentInChildren<Image>());
            itemCount++;
        }

        //TODO handle 0 amount, remove icon
        bankText[slotIndex].text = amount.ToString();
        bankImage[slotIndex].sprite = Resources.Load<Sprite>("Icons/" + id.ToString());
    }
}
