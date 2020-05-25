using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrainingMethodScrollView : MonoBehaviour
{
    public GameObject buttonPrefab;
    public Transform buttonListParent;

    private List<GameObject> buttonGameObjects;

    private void Start()
    {
        buttonGameObjects = new List<GameObject>();
        EventManager.Instance.OnDrawTrainingMethods += CreateTrainingMethodButtons;

        gameObject.SetActive(false);
    }

    private void CreateTrainingMethodButtons(List<TrainingMethod> trainingMethodList)
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

            button.transform.SetParent(buttonListParent, false);

            button.GetComponent<Button>().onClick.AddListener(() => EventManager.Instance.TrainingMethodClicked(trainingMethodButton.index));
            index++;
        }
    }
}
