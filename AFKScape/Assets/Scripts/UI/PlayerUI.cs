using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    GameObject playerUi;
    // Start is called before the first frame update
    void Start()
    {
        playerUi = gameObject;
        playerUi.SetActive(true);
        EventManager.Instance.OnMainTabClicked += ShowHidePlayerUI;
    }

    private void ShowHidePlayerUI(int index)
    {
        if (index == 5)
        {
            playerUi.SetActive(false);
        }
        else
        {
            playerUi.SetActive(true);
        }
    }
}
