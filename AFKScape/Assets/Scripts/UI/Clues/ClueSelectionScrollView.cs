using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClueSelectionScrollView : MonoBehaviour
{
    public GameObject buttonPrefab;
    public Transform buttonListParent;

    private List<GameObject> buttonGameObjects;
    private readonly List<string> cluesNames = new List<string> {
        "Beginner",
        "Easy",
        "Medium",
        "Hard",
        "Elite",
        "Master",
    };

    // Start is called before the first frame update
    private void Start()
    {
        buttonGameObjects = new List<GameObject>();
        CreateBossSelectionButtons(cluesNames);
    }

    public void CreateBossSelectionButtons(List<string> clues)
    {
        foreach (string name in clues)
        {
            GameObject buttonObj = Instantiate(buttonPrefab);
            buttonGameObjects.Add(buttonObj);
            buttonObj.SetActive(true);

            buttonObj.transform.SetParent(buttonListParent, false);

            Button button = buttonObj.GetComponent<Button>();
            button.onClick.AddListener(() => EventManager.Instance.ClueClicked(name));
            button.GetComponentInChildren<Text>().text = name;
        }
    }
}
