using UnityEngine;
using UnityEngine.UI;
public class TrainingMethodButton : MonoBehaviour
{
    public Text buttonText;

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
