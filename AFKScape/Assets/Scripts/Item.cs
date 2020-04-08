using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    private string name;
    private string category;
    private int price;

    private struct Stats //todo fill
    {
        public int stab;
        public int crush;
        public int slash;

        public int prayer;
    }

    private Stats stats;

    private bool isEquipable;
    private bool isConsumable;
    private bool isStackable;

    private struct Requirements
    {
        public List<LevelRequirement> levelRequirements;
        public List<string> quest;
    }

    private Requirements requirements;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
