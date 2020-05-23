using UnityEngine;
using UnityEngine.UI;

public class PopUpMessage : MonoBehaviour
{
    public Text textObject;

    void Start()
    {
        EventManager.Instance.OnShowPopUpMsg += ShowMessage;
        gameObject.SetActive(false);
    }

    public void ShowMessage(string msg)
    {
        gameObject.SetActive(true);
        textObject.text = msg;
    }
}
