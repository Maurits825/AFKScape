using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour, IDropHandler
{
    private const int NUMCOLS = 4;
    private const int NUMROWS = 7;

    [SerializeField]
    private GameObject slotPrefab;
    [SerializeField]
    private Transform slotListParent;

    private float widthOffset;
    private float heightOffset;
    private Transform inventoryTransform;
    private Rect inventoryRect;

    private int nextAvailableSlot = 0;
    private List<GameObject> slotsObjects = new List<GameObject>();
    private Dictionary<long, Text> inventoryText = new Dictionary<long, Text>();
    private Dictionary<long, Image> inventoryImage = new Dictionary<long, Image>();
    private Dictionary<long, int> slotIndexes = new Dictionary<long, int>();
    private List<bool> isUsed = new List<bool>();

    void Awake()
    {
        inventoryTransform = GetComponent<Transform>();
        inventoryRect = GetComponent<RectTransform>().rect;

        GridLayoutGroup gridLayoutGroup = GetComponentInChildren<GridLayoutGroup>();
        widthOffset = (inventoryRect.width - ((gridLayoutGroup.cellSize.x * NUMCOLS) + (gridLayoutGroup.spacing.x * (NUMCOLS - 1)))) / 2;
        heightOffset = (inventoryRect.height - ((gridLayoutGroup.cellSize.y * NUMROWS) + (gridLayoutGroup.spacing.y * (NUMROWS - 1)))) / 2;
    }

    // Start is called before the first frame update
    void Start()
    {
        EventManager.Instance.onItemChanged += UpdateInventoryUI;
        InitUI();
    }

    public void InitUI()
    {
        for (int i = 0; i < 28; i++) //TODO magic num 28
        {
            GameObject slotObject = Instantiate(slotPrefab) as GameObject;
            slotObject.transform.SetParent(slotListParent, false);
            slotsObjects.Add(slotObject);
            Slot slot = slotObject.GetComponent<Slot>();
            slot.SetSlotActive(false);

            isUsed.Add(false);
        }
    }

    private void UpdateInventoryUI(long id, int amount)
    {
        if (!inventoryText.ContainsKey(id))
        {
            Slot slot = slotsObjects[nextAvailableSlot].GetComponent<Slot>();
            slot.SetItemName(Database.items[id].name);
            slot.isDraggable = true;
            slot.SetSlotActive(true);

            inventoryText.Add(id, slot.amountText);
            inventoryImage.Add(id, slot.iconImage);

            isUsed[nextAvailableSlot] = true;
            UpdateNextAvailableSlot();
        }

        inventoryText[id].text = amount.ToString();
        inventoryImage[id].sprite = Resources.Load<Sprite>("Icons/" + id.ToString());
    }

    public void OnDrop(PointerEventData eventData)
    {
        Vector2 pos = eventData.position;
        int indexDropped = GetGridLinearIndex(pos.x, pos.y);

        Transform toSwap = slotListParent.GetChild(indexDropped);
        Transform itemDropped = eventData.pointerDrag.transform;
        int indexToSwap = itemDropped.GetSiblingIndex();

        itemDropped.SetSiblingIndex(indexDropped);
        toSwap.SetSiblingIndex(indexToSwap);

        LayoutRebuilder.MarkLayoutForRebuild(slotListParent.GetComponent<RectTransform>());

        bool temp = isUsed[indexDropped];
        isUsed[indexDropped] = isUsed[indexToSwap];
        isUsed[indexToSwap] = temp;

        GameObject tempObj = slotsObjects[indexDropped];
        slotsObjects[indexDropped] = slotsObjects[indexToSwap];
        slotsObjects[indexToSwap] = tempObj;

        UpdateNextAvailableSlot();
    }

    private void UpdateNextAvailableSlot()
    {
        nextAvailableSlot = 0;
        while (isUsed[nextAvailableSlot] && nextAvailableSlot < (isUsed.Count - 1))
        {
            nextAvailableSlot++;
        }
    }

    private int GetGridLinearIndex(float x, float y)
    {
        float localX = x - inventoryTransform.position.x + (inventoryRect.width / 2);
        float localY = y - inventoryTransform.position.y + (inventoryRect.height / 2);

        int indX = 0;
        int indY = 0;

        if (localX <= widthOffset)
        {
            indX = 0;
        }
        else if (localX >= (inventoryRect.width - widthOffset))
        {
            indX = NUMCOLS - 1;
        }
        else
        {
            indX = Mathf.FloorToInt(((localX - widthOffset) / (inventoryRect.width - 2*widthOffset)) * NUMCOLS);
        }

        if (localY <= heightOffset)
        {
            indY = NUMROWS - 1;
        }
        else if (localY >= (inventoryRect.height - heightOffset))
        {
            indY = 0;
        }
        else
        {
            indY = (NUMROWS - 1) - Mathf.FloorToInt(((localY - heightOffset) / (inventoryRect.height - 2*heightOffset)) * NUMROWS);
        }

        return GetLinearIndex(indX, indY, NUMCOLS);
    }

    private int GetLinearIndex(int x, int y, int width)
    {
        return x + (y * width);
    }
}
