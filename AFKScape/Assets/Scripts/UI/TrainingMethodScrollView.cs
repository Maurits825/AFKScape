using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingMethodScrollView : MonoBehaviour
{
    [SerializeField]
    private GameObject buttonPrefab;

    private List<GameObject> buttonGameObjects;
    // Start is called before the first frame update
    void Start()
    {
        buttonGameObjects = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
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

            button.GetComponent<TrainingMethodButton>().SetText(trainingMethod.name);
            button.GetComponent<TrainingMethodButton>().SetIndex(index);

            button.transform.SetParent(transform, false);
        }
    }
}
