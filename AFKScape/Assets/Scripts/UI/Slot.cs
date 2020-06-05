using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler
{
    public Text amountText;
    public Image iconImage;

    public bool isDraggable = false;

    public Text toolTipText;
    public GameObject toolTipObject;

    public long id;

    private Transform parentTransform;
    private float yMin;
    private float yMax;
    private float xMin;
    private float xMax;

    private Transform toolTipTransform;
    private bool isOver;

    private CanvasGroup canvasGroup;

    private static readonly float YPosOffset = -65.0F;

    private float textHeight = 0;
    private float textWidth = 0;

    private Vector2 mouseOffset = Vector2.zero;

    private Vector2 initPos;

    public enum State
    {
        Bank,
        Inventory,
        Equipped,
    }

    private State state;

    private void Start()
    {
        canvasGroup = gameObject.GetComponent<CanvasGroup>();
        Rect parentRect;

        parentTransform = transform.parent.gameObject.transform;
        parentRect = transform.parent.GetComponent<RectTransform>().rect;
        GridLayoutGroup gridLayoutGroup = GetComponentInParent<GridLayoutGroup>();
        xMin = parentTransform.position.x - (parentRect.width / 2) + (gridLayoutGroup.cellSize.x / 2);
        xMax = parentTransform.position.x + (parentRect.width / 2) - (gridLayoutGroup.cellSize.x / 2);
        yMin = parentTransform.position.y - (parentRect.height / 2) + (gridLayoutGroup.cellSize.y / 2);
        yMax = parentTransform.position.y + (parentRect.height / 2) - (gridLayoutGroup.cellSize.y / 2);

        toolTipObject.SetActive(false);
        toolTipTransform = toolTipObject.transform;
        isOver = false;
    }

    private void Update()
    {
        if (textHeight == 0)
        {
            Rect textRect = toolTipObject.GetComponent<RectTransform>().rect;
            textHeight = textRect.height;
            textWidth = textRect.width;
        }

        if (isOver)
        {
            toolTipTransform.position = new Vector3(
                Mathf.Clamp(Input.mousePosition.x, 0.0F, Screen.width - textWidth),
                Mathf.Clamp(Input.mousePosition.y + YPosOffset, 0.0F + (textHeight / 2.0F), Screen.height),
                0.0F);
        }
    }

    public void SetItemName(string itemName)
    {
        toolTipText.text = itemName;
    }

    public void SetId(long val)
    {
        id = val;
    }

    public void SetAlpha(float value)
    {
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.GetComponent<CanvasGroup>();
        }

        canvasGroup.alpha = value;
    }

    public void SetSlotActive(bool activate)
    {
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.GetComponent<CanvasGroup>();
        }

        if (activate)
        {
            canvasGroup.interactable = true;
            canvasGroup.alpha = 1F;
            canvasGroup.blocksRaycasts = true;
        }
        else
        {
            canvasGroup.interactable = false;
            canvasGroup.alpha = 0F;
            canvasGroup.blocksRaycasts = false;
        }
    }

    public void SetState(State newState)
    {
        state = newState;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        toolTipObject.SetActive(true);
        isOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        toolTipObject.SetActive(false);
        isOver = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDraggable)
        {
            Vector2 pos = eventData.position + mouseOffset;
            initPos = gameObject.transform.position;
            transform.position = new Vector2(Mathf.Clamp(pos.x, xMin, xMax), Mathf.Clamp(pos.y, yMin, yMax));
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isDraggable)
        {
            canvasGroup.alpha = 0.6F;
            mouseOffset = (Vector2)transform.position - eventData.position;
            canvasGroup.blocksRaycasts = false;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isDraggable)
        {
            Vector2 mousePos = eventData.position;
            if (mousePos.x > xMax || mousePos.x < xMin || mousePos.y > yMax || mousePos.y < yMin)
            {
                gameObject.transform.position = initPos;
            }

            canvasGroup.alpha = 1F;
            canvasGroup.blocksRaycasts = true;

            LayoutRebuilder.MarkLayoutForRebuild(parentTransform.GetComponent<RectTransform>());
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        EventManager.Instance.SlotClicked(state, id);
        Debug.Log("Slot clicked!");
    }
}
