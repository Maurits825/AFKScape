using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour, IDropHandler
{
    private const int NUMCOLS = 4;
    private const int NUMROWS = 7;

    public GameObject slotPrefab;
    public Transform slotListParent;

    private float widthOffset;
    private float heightOffset;
    private Transform inventoryTransform;
    private Rect inventoryRect;

    //TODO simplify this mess?
    private int nextAvailableSlot = 0;
    private List<Slot> slots = new List<Slot>();
    private Dictionary<long, Text> inventoryText = new Dictionary<long, Text>();
    private Dictionary<long, Image> inventoryImage = new Dictionary<long, Image>();
    private Dictionary<long, int> idIndex = new Dictionary<long, int>();
    private Dictionary<int, long> indexId = new Dictionary<int, long>();
    private List<bool> isUsed = new List<bool>();

    private void Awake()
    {
        inventoryTransform = GetComponent<Transform>();
        inventoryRect = GetComponent<RectTransform>().rect;

        GridLayoutGroup gridLayoutGroup = GetComponentInChildren<GridLayoutGroup>();
        widthOffset = (inventoryRect.width - ((gridLayoutGroup.cellSize.x * NUMCOLS) + (gridLayoutGroup.spacing.x * (NUMCOLS - 1)))) / 2;
        heightOffset = (inventoryRect.height - ((gridLayoutGroup.cellSize.y * NUMROWS) + (gridLayoutGroup.spacing.y * (NUMROWS - 1)))) / 2;
    }

    // Start is called before the first frame update
    private void Start()
    {
        EventManager.Instance.OnInvItemAdded += InventoryItemAdded;
        EventManager.Instance.OnInvItemRemoved += InventoryItemRemoved;
        InitUI();
    }

    public void InitUI()
    {
        //TODO magic num 28
        for (int i = 0; i < 28; i++)
        {
            GameObject slotObject = Instantiate(slotPrefab);
            slotObject.transform.SetParent(slotListParent, false);

            Slot slot = slotObject.GetComponent<Slot>();
            slot.SetSlotActive(false);
            slot.isDraggable = true;
            slots.Add(slot);

            indexId[i] = 0;
            isUsed.Add(false);
        }
    }

    private void InventoryItemAdded(long id, BigInteger amount)
    {
        if (amount > 0)
        {
            if (!inventoryText.ContainsKey(id))
            {
                Slot slot = slots[nextAvailableSlot];
                slot.SetItemName(Database.items[id].name);
                slot.SetId(id);
                slot.SetState(Slot.State.Inventory);
                slot.SetSlotActive(true);

                inventoryText.Add(id, slot.amountText);
                inventoryImage.Add(id, slot.iconImage);
                inventoryImage[id].sprite = Database.sprites[(int)id];

                isUsed[nextAvailableSlot] = true;
                idIndex[id] = nextAvailableSlot;
                indexId[nextAvailableSlot] = id;

                UpdateNextAvailableSlot();
            }

            (inventoryText[id].text, inventoryText[id].color) = UtilityUI.FormatNumber(amount);
        }
    }

    private void InventoryItemRemoved(long id, BigInteger amount)
    {
        if (inventoryText.ContainsKey(id))
        {
            if (amount == 0)
            {
                int index = idIndex[id];
                Slot slot = slots[index];
                slot.SetSlotActive(false);

                inventoryText.Remove(id);
                inventoryImage.Remove(id);

                isUsed[index] = false;
                UpdateNextAvailableSlot();
            }
            else
            {
                inventoryText[id].text = amount.ToString();
            }
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        UnityEngine.Vector2 pos = eventData.position;
        int indexDropped = GetGridLinearIndex(pos.x, pos.y);

        Transform toSwap = slotListParent.GetChild(indexDropped);
        Transform itemDropped = eventData.pointerDrag.transform;
        int indexToSwap = itemDropped.GetSiblingIndex();

        itemDropped.SetSiblingIndex(indexDropped);
        toSwap.SetSiblingIndex(indexToSwap);

        LayoutRebuilder.MarkLayoutForRebuild(slotListParent.GetComponent<RectTransform>());

        (isUsed[indexDropped], isUsed[indexToSwap]) = (isUsed[indexToSwap], isUsed[indexDropped]);
        (slots[indexDropped], slots[indexToSwap]) = (slots[indexToSwap], slots[indexDropped]);

        (indexId[indexDropped], indexId[indexToSwap]) = (indexId[indexToSwap], indexId[indexDropped]);
        idIndex[indexId[indexDropped]] = indexDropped;
        idIndex[indexId[indexToSwap]] = indexToSwap;

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

        int indX;
        int indY;

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
            indX = Mathf.FloorToInt(((localX - widthOffset) / (inventoryRect.width - (2 * widthOffset))) * NUMCOLS);
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
            indY = (NUMROWS - 1) - Mathf.FloorToInt(((localY - heightOffset) / (inventoryRect.height - (2 * heightOffset))) * NUMROWS);
        }

        return GetLinearIndex(indX, indY, NUMCOLS);
    }

    private int GetLinearIndex(int x, int y, int width)
    {
        return x + (y * width);
    }
}
