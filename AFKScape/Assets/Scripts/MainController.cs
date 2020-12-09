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
    public CluesController cluesController;

    public ItemManager itemManager;
    public Inventory inventory;
    public Bank bank;
    public Equipment equipment;

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
                cluesController.Operate();
                break;

            case States.CollectionLog:
                break;

            default:
                break;
        }
    }

    private void SetInstances()
    {
        inventory = new Inventory();
        bank = new Bank();

        skillsController = new SkillsController(inventory, bank);
        bossesController = new BossesController(inventory, bank);
        cluesController = new CluesController(inventory, bank);

        equipment = new Equipment(inventory);
        itemManager = new ItemManager(inventory, bank, equipment);
    }

    private void OnTabClicked(int index)
    {
        gameState = (States)index;
    }
}
