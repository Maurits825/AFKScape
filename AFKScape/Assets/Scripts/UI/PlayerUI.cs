using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    private GameObject playerUi;

    private void Start()
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
