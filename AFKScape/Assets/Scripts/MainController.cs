using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainController : MonoBehaviour
{
    private enum States
    {
        Skills = 0,
        Quests,
        Bosses,
        Raids,
        Clues,
        CollectionLog,
        Initializing,
        Idle,
    }

    private States gameState = States.Initializing;

    //move this somewhere else?
    public static float timeConstant = (1.0F / (60.0F * 60.0F)) * 10;

    //all class "singleton" instances, make static?
    public SkillsController skillsController;

    public Inventory inventory;
    public Bank bank;

    
    // Start is called before the first frame update
    void Start()
    {
        SetInstances();
        Database.LoadAll();

        EventManager.Instance.onMainTabClicked += OnTabClicked;

        //call init on all classes
        skillsController.Initialize(inventory);

        //default tab is skills
        gameState = States.Skills;
    }

    // Update is called once per frame
    void Update()
    {
        switch (gameState)
        {
            case States.Initializing:
                break;

            case States.Idle:
                break;

            case States.Skills:
                skillsController.Operate();
                break;

            case States.Quests:
                break;

            case States.Bosses:
                break;

            case States.Raids:
                break;

            case States.Clues:
                break;

            case States.CollectionLog:
                break;

            default:
                break;
        }
    }

    private void SetInstances()
    {
        skillsController = new SkillsController();
        inventory = new Inventory();
        bank = new Bank();
    }

    private void OnTabClicked(int index)
    {
        gameState = (States)index;
    }
}
