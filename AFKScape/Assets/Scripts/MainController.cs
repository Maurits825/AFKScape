using UnityEngine;

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
    public BossesController bossesController;

    public Inventory inventory;
    public Bank bank;

    private void Awake()
    {
        EventManager.SetIntance(new EventManager());
        Database.LoadAll();
    }

    // Start is called before the first frame update
    private void Start()
    {
        SetInstances();

        EventManager.Instance.OnMainTabClicked += OnTabClicked;

        //call init on all classes
        skillsController.Initialize(inventory, bank);
        bossesController.Initialize(inventory, bank);

        //default tab is skills
        gameState = States.Skills;
    }

    // Update is called once per frame
    private void Update()
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
                bossesController.Operate();
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
        bossesController = new BossesController();

        inventory = new Inventory();
        bank = new Bank();
    }

    private void OnTabClicked(int index)
    {
        gameState = (States)index;
    }
}
