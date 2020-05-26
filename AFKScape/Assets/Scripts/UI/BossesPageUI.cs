using UnityEngine;
using UnityEngine.UI;

public class BossesPageUI : MonoBehaviour
{
    public GameObject BossSelectionPanel;
    public GameObject animationObj;
    public Button newBossButton;

    public Text status;
    public Text killCountText;

    private BossSelectionScrollView bossSelectionScrollView;

    private void Awake()
    {
        bossSelectionScrollView = BossSelectionPanel.GetComponent<BossSelectionScrollView>();
    }

    private void Start()
    {
        EventManager.Instance.OnBossClicked += BossClicked;
        EventManager.Instance.OnBossKilled += BossKilled;

        newBossButton.onClick.AddListener(() => EventManager.Instance.BossClicked(null));

        status.text = string.Empty;
    }

    private void BossClicked(string bossName)
    {
        if (string.IsNullOrEmpty(bossName))
        {
            BossSelectionPanel.SetActive(true);
            animationObj.SetActive(false);
        }
        else
        {
            BossSelectionPanel.SetActive(false);
            animationObj.SetActive(true);
        }

        status.text = bossName;
    }

    private void BossKilled(int killCount)
    {
        killCountText.text = "Kill Count: " + killCount.ToString();
    }
}
