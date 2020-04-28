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

    private int itemCount = 0;
    private int slotIndex = 0;
    private List<GameObject> slotsObjecs = new List<GameObject>();
    private Dictionary<long, Text> bankText = new Dictionary<long, Text>();
    private Dictionary<long, Image> bankImage = new Dictionary<long, Image>();

    void Start()
    {
        EventManager.Instance.onBankItemAdded += UpdateLastLootUI;
    }

    void UpdateLastLootUI(long id, int amount, int _)
    {
        if (!bankText.ContainsKey(id))
        {
            GameObject slot = Instantiate(slotPrefab) as GameObject;
            slot.transform.SetParent(slotListParent, false);
            slotsObjecs.Add(slot);
            bankText.Add(id, slot.GetComponentInChildren<Text>());
            bankImage.Add(id, slot.GetComponentInChildren<Image>());
            itemCount++;
        }

        bankText[id].text = amount.ToString();
        bankImage[id].sprite = Resources.Load<Sprite>("Icons/" + id.ToString());
    }
}
