using UnityEngine;
using UnityEngine.UI;

public class PopUpMessage : MonoBehaviour
{
    public Text textObject;

    public void ShowMessage(string msg)
    {
        gameObject.SetActive(true);
        textObject.text = msg;
    }

    private void Start()
    {
        EventManager.Instance.OnShowPopUpMsg += ShowMessage;
        gameObject.SetActive(false);
    }
}
