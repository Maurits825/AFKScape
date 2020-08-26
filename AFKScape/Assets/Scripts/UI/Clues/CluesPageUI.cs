using UnityEngine;
using UnityEngine.UI;

public class CluesPageUI : MonoBehaviour
{
    public GameObject ClueSelectionPanel;
    public GameObject animationObj;
    public Button newClueButton;

    public Text status;
    public Text killCountText;

    private ClueSelectionScrollView clueSelectionScrollView;

    private void Awake()
    {
        clueSelectionScrollView = ClueSelectionPanel.GetComponent<ClueSelectionScrollView>();
    }

    private void Start()
    {
        EventManager.Instance.OnClueClicked += ClueClicked;
        EventManager.Instance.OnClueCompleted += ClueCompleted;

        newClueButton.onClick.AddListener(() => EventManager.Instance.ClueClicked(null));

        status.text = string.Empty;
    }

    private void ClueClicked(string clueName)
    {
        if (string.IsNullOrEmpty(clueName))
        {
            ClueSelectionPanel.SetActive(true);
            animationObj.SetActive(false);
        }
        else
        {
            ClueSelectionPanel.SetActive(false);
            animationObj.SetActive(true);
        }

        status.text = clueName;
    }

    private void ClueCompleted(int killCount)
    {
        killCountText.text = "Count: " + killCount.ToString();
    }
}
