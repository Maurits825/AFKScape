using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TrainingMethodScrollView : MonoBehaviour
{
    [SerializeField]
    private GameObject buttonPrefab;

    private List<GameObject> buttonGameObjects;
    // Start is called before the first frame update
    void Start()
    {
        buttonGameObjects = new List<GameObject>();
        EventManager.Instance.OnDrawTrainingMethods += CreateTrainingMethodButtons;
    }

    public void CreateTrainingMethodButtons(List<TrainingMethod> trainingMethodList)
    {
        //TODO dynamically Destroy and Instantiate, could be improved for better performance by storing all in mem
        if (buttonGameObjects.Count > 0)
        {
            foreach (GameObject button in buttonGameObjects)
            {
                Destroy(button);
            }

            buttonGameObjects.Clear();
        }

        int index = 0;
        foreach (TrainingMethod trainingMethod in trainingMethodList)
        {
            GameObject button = Instantiate(buttonPrefab) as GameObject;
            buttonGameObjects.Add(button);
            button.SetActive(true);

            TrainingMethodButton trainingMethodButton = button.GetComponent<TrainingMethodButton>();
            trainingMethodButton.SetText(trainingMethod.requirements.levelRequirements[0].levelReq, trainingMethod.name);
            trainingMethodButton.SetIndex(index);

            button.transform.SetParent(transform, false);

            button.GetComponent<Button>().onClick.AddListener(() => EventManager.Instance.TrainingMethodClicked(trainingMethodButton.index));
            index++;
        }
    }
}
