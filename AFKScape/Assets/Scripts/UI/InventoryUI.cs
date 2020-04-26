using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel;
    private Dictionary<int, Text> inventoryText = new Dictionary<int, Text>();
    private Dictionary<int, Image> inventoryImage = new Dictionary<int, Image>();

    public void InitUI()
    {
        Text[] invTexts = inventoryPanel.GetComponentsInChildren<Text>();
        Image[] invImages = inventoryPanel.GetComponentsInChildren<Image>();

        int slot = 0;
        foreach (Text invText in invTexts)
        {
            inventoryText.Add(slot, invText);
            slot++;
        }

        slot = 0;
        foreach (Image invImage in invImages)
        {
            inventoryImage.Add(slot, invImage);
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
            inventoryText[slotIndex].text = amount.ToString();
            inventoryImage[slotIndex].sprite = Resources.Load<Sprite>("Icons/" + id.ToString());
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        EventManager.Instance.onItemChanged += UpdateInventoryUI;
        InitUI();
    }
}
