using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TrainingMethodButton : MonoBehaviour
{
    [SerializeField]
    private Text buttonText;

    private int index;

    public void SetIndex(int i)
    {
        index = i;
    }

    public void SetText(string text)
    {
        buttonText.text = text;
    }

    //TODO better way? maincontroller as singleton maybe, or static, or have instance manager
    public void HandleClick()
    {
        GameObject.Find("MainController").GetComponent<MainController>().SetTrainingMethod(index);
    }
}
