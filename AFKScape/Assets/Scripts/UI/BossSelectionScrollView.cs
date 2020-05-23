using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossSelectionScrollView : MonoBehaviour
{
    public GameObject buttonPrefab;
    public Transform buttonListParent;

    private List<GameObject> buttonGameObjects;

    // Start is called before the first frame update
    void Start()
    {
        buttonGameObjects = new List<GameObject>();
        CreateBossSelectionButtons(Database.bossesNames);
    }

    public void CreateBossSelectionButtons(List<string> bossesNames)
    {
        foreach (string name in bossesNames)
        {
            GameObject buttonObj = Instantiate(buttonPrefab) as GameObject;
            buttonGameObjects.Add(buttonObj);
            buttonObj.SetActive(true);

            buttonObj.transform.SetParent(buttonListParent, false);

            Button button = buttonObj.GetComponent<Button>();
            button.onClick.AddListener(() => EventManager.Instance.BossClicked(name));
            button.GetComponentInChildren<Text>().text = name;
        }
    }
}
