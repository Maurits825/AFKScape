using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TrainingMethodButton : MonoBehaviour
{
    [SerializeField]
    private Text buttonText;

    public int index;

    public void SetIndex(int i)
    {
        index = i;
    }

    public void SetText(int lvl, string text)
    {
        buttonText.text = lvl + ", " + text;
    }
}
