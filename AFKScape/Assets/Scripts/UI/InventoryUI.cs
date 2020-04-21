using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel;
    private Dictionary<int, Text> inventoryText = new Dictionary<int, Text>();

    public void InitUI()
    {
        Text[] invTexts = inventoryPanel.GetComponentsInChildren<Text>();
        int slot = 0;
        foreach (Text invText in invTexts)
        {
            inventoryText.Add(slot, invText);
            slot++;
        }
    }

    private void UpdateInventoryUI(long id, int amount, int slotIndex)
    {
        if (amount <= 0)
        {
            inventoryText[slotIndex].text = "";
        }
        else
        {
            inventoryText[slotIndex].text = string.Concat(JsonHandler.items[id].name, "\n", amount.ToString());
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        EventManager.Instance.onItemChanged += UpdateInventoryUI;
        InitUI();
    }
}
