using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastLootScrollView
{
    [SerializeField]
    private GameObject slotPrefab;

    private List<GameObject> slotList;

    // Start is called before the first frame update
    void Start()
    {
        //EventManager.Instance.onDrawTrainingMethods += CreateTrainingMethodButtons;
        //onNewItemReceived/onNewItemAdded += updateamount/updateUI
    }

    //void update ui(int id, int amount, ind slotIndex)
    //from inventory ui
    //...Text[slotIndex] = id.ToString
    //...Image = load(id.ToString)

    void CreateLootPlaceHodlers(Dictionary<long, ItemSlot> items)
    {
        //foreach item in items
        //instantiate slotPrefab
        //add slot to list
        //then we can update with slotList[index].text?
        //or imageList.add(slot.image) or something
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
