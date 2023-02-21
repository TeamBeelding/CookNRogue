using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class LeoScrolling : MonoBehaviour
{
    public RectTransform container;
    private RectTransform[] Items;
    bool m_LerpRight = false;
    bool m_LerpLeft = false;
    private float m_padding;
    
    private Vector3[] targets;

    // Start is called before the first frame update
    void Start()
    {
        Items = new RectTransform[container.childCount];
        for (int i = 0; i < Items.Length; i++)
        {
            Items[i] = container.GetChild(i).GetComponent<RectTransform>();
        }
        m_padding = container.GetComponent<HorizontalLayoutGroup>().spacing;

        targets = new Vector3[container.childCount];
        for (int i = 0; i < targets.Length; i++)
        {
            targets[i] = Items[i].GetComponent<RectTransform>().localPosition;
            Debug.Log(targets[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Debug.Log("right");
            StartCoroutine(lerpRight(0.5f));
            for (int i = 0; i < targets.Length; i++)
            {
                targets[i] += new Vector3(m_padding,0,0); 
            }

        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Debug.Log("left");
            StartCoroutine(lerpLeft(0.5f));
            for (int i = 0; i < targets.Length; i++)
            {
                targets[i] -= new Vector3(m_padding, 0, 0);
            }
        }
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < Items.Length; i++)
        {
            Items[i].localPosition = Vector3.Lerp(Items[i].localPosition, targets[i],0.5f);
        }
       
    }
    IEnumerator lerpRight(float time)
    {
        m_LerpRight = true;
        yield return new WaitForSeconds(time);
        m_LerpRight = false;
    }
    IEnumerator lerpLeft(float time)
    {
        m_LerpLeft = true;
        yield return new WaitForSeconds(time);
        m_LerpLeft = false;
    }
}
