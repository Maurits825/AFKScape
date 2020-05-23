using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Image mask;
    public Image fill;
    public Image icon;
    public Color barColor;

    private int minimum;
    private int maximum;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    public void SetIcon(string iconName)
    {
        icon.sprite = Resources.Load<Sprite>(iconName);
    }

    public void InitProgressBar(int min, int max)
    {
        minimum = min;
        maximum = max;
    }

    public void UpdateProgressBar(int current)
    {
        float currentOffset = current - minimum;
        float maximumOffset = maximum - minimum;
        float fillAmount = currentOffset / maximumOffset;
        mask.fillAmount = fillAmount;

        fill.color = barColor;
    }
}
