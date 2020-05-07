using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XpDrop : MonoBehaviour
{
    public Text xpText;
    public Image icon;
    public GameObject xpDropObj;

    private Vector3 startPos;
    private Vector3 target;
    private float speed = 170;
    private float spacing;

    private Queue<GameObject> xpDropActive;
    private Queue<GameObject> xpDropPool;
    private int poolSize = 10;
    private GameObject lastObj;

    private int xpCummulative;

    // Start is called before the first frame update
    void Start()
    {
        float xpDropheight = xpDropObj.GetComponent<RectTransform>().rect.height;
        float areaHeight = GetComponent<RectTransform>().rect.height;
        target = new Vector3(0, areaHeight/2 + xpDropheight/2, 0);
        startPos = new Vector3(0, 0 - areaHeight/2 - xpDropheight/2, 0);
        xpCummulative = 0;

        spacing = startPos.y + xpDropheight;

        xpDropActive = new Queue<GameObject>();
        xpDropPool = new Queue<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(xpDropObj) as GameObject;
            xpDropPool.Enqueue(obj);
            obj.transform.SetParent(transform, false);
            obj.transform.localPosition = startPos;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float step = speed * Time.deltaTime;

        if (xpDropActive.Count != 0)
        {
            foreach (GameObject obj in xpDropActive)
            {
                obj.transform.localPosition += new Vector3(0, step, 0);
            }

            if (xpDropActive.Peek().transform.localPosition.y >= target.y)
            {
                GameObject obj = xpDropActive.Dequeue();
                xpDropPool.Enqueue(obj);
            }
        }
        
    }

    public void StartXpDrop(string skillIcon, int xp)
    {
        xpCummulative += xp;

        if (xpDropActive.Count == 0 || (lastObj != null && lastObj.transform.localPosition.y >= spacing))
        {
            GameObject obj = xpDropPool.Dequeue();
            xpDropActive.Enqueue(obj);
            lastObj = obj;

            obj.GetComponentInChildren<Text>().text = xpCummulative.ToString();
            obj.GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>(skillIcon);
            obj.transform.localPosition = startPos;

            xpCummulative = 0;
        }
    }
}
