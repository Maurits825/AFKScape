using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XpDrop : MonoBehaviour
{
    public Text xpText;
    public Image icon;
    public GameObject xpDropObj;

    private List<GameObject> xpDropObjs = new List<GameObject>();
    private float heightOffset;
    private Vector3 startPos;
    private Vector3 target;
    private float speed = 150;

    // Start is called before the first frame update
    void Start()
    {
        float xpDropheight = xpDropObj.GetComponent<RectTransform>().rect.height;
        float areaHeight = GetComponent<RectTransform>().rect.height;
        target = new Vector3(0, areaHeight + heightOffset, 0);
        startPos = new Vector3(0, 0 - areaHeight/2 - xpDropheight/2, 0);
    }

    // Update is called once per frame
    void Update()
    {
        float step = speed * Time.deltaTime;
        for (int i = 0; i < xpDropObjs.Count; i++)
        {
            xpDropObjs[i].transform.localPosition += new Vector3(0, step, 0);
        }
    }

    public void StartXpDrop(string skillIcon, int xp)
    {

        GameObject xpDropObjClone = Instantiate(xpDropObj) as GameObject;
        xpDropObjs.Add(xpDropObjClone);
        xpDropObjClone.transform.SetParent(transform, false);

        xpDropObjClone.GetComponentInChildren<Text>().text = xp.ToString();
        xpDropObjClone.GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>(skillIcon);
        xpDropObjClone.transform.localPosition = startPos;
    }
}
