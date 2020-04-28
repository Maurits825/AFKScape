using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Text amountText;
    public Image iconImage;

    [SerializeField]
    private Text toolTipText;
    [SerializeField]
    private GameObject toolTipObject;

    private Transform toolTipTransform;
    private bool isOver;

    private static readonly float yPosoffset = -65.0F;
    private static float textHeight = 50; //TODO idk how to get this dynamically
    
    void Start()
    {
        toolTipObject.SetActive(false);
        toolTipTransform = toolTipObject.transform;
        isOver = false;
    }

    void Update()
    {
        if (isOver)
        {
            toolTipTransform.position = new Vector3(Input.mousePosition.x, Mathf.Clamp(Input.mousePosition.y + yPosoffset, 0.0F + textHeight/2.0F, Screen.height), 1.0F);
        }
    }

    public void SetItemName(string itemName)
    {
        toolTipText.text = itemName;
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
}
