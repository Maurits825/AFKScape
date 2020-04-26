using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    GameObject playerUi;
    // Start is called before the first frame update
    void Start()
    {
        playerUi = gameObject;
        playerUi.SetActive(true);
        EventManager.Instance.onMainTabClicked += ShowHidePlayerUI;
    }

    // Update is called once per frame
    void Update()
    {
        
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
