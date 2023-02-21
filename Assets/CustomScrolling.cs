using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

[RequireComponent(typeof(ScrollRect))]
public class CustomScrolling : MonoBehaviour
{
    [Tooltip("the container the screens or pages belong to")]
    public Transform ScreensContainer;
    [Tooltip("how many screens or pages are there wit$$anonymous$$n the content")]
    public int Screens = 1;
    [Tooltip("w$$anonymous$$ch screen or page to start on")]
    public int StartingScreen = 1;

    private List<Vector3> m_Positions;
    private ScrollRect m_ScrollRect;
    private Vector3 m_LerpTarget;
    private bool m_Lerp;
    private RectTransform m_ScrollViewRectTrans;
    private float m_padding;

    void Start()
    {
        m_ScrollRect = gameObject.GetComponent<ScrollRect>();
        m_ScrollViewRectTrans = gameObject.GetComponent<RectTransform>();
        m_ScrollRect.inertia = false;
        m_Lerp = false;
        m_padding = ScreensContainer.GetComponent<HorizontalLayoutGroup>().spacing;
        m_Positions = new List<Vector3>();

        if (Screens > 0)
        {
            Vector3 startPos = ScreensContainer.localPosition;
            Vector3 endPos = ScreensContainer.localPosition + Vector3.left * ((Screens - 1) * (m_ScrollViewRectTrans.rect.width + m_padding));

            for (int i = 0; i < Screens; ++i)
            {
                float horiNormPos = (float)i / (float)(Screens - 1);
                // t$$anonymous$$s does not seem to have an effect [Tested on Unity 4.6.0 RC 2]
                m_ScrollRect.horizontalNormalizedPosition = horiNormPos;
                m_Positions.Add(Vector3.Lerp(startPos, endPos, horiNormPos));
            }
        }

        // t$$anonymous$$s does not seem to have an effect [Tested on Unity 4.6.0 RC 2]
        m_ScrollRect.horizontalNormalizedPosition = (float)(StartingScreen - 1) / (float)(Screens - 1);
    }


    private void Update()
    {
      
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Debug.Log("right");
            StartCoroutine(lerpTime(0.5f));
            m_LerpTarget = FindClosestFrom(ScreensContainer.localPosition - new Vector3(100,0,0), m_Positions);
            if(m_LerpTarget == ScreensContainer.localPosition)
            {
                m_LerpTarget = m_Positions[Screens - 1];
            }

        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Debug.Log("left");
            StartCoroutine(lerpTime(0.5f));
            m_LerpTarget = FindClosestFrom(ScreensContainer.localPosition + new Vector3(100, 0, 0), m_Positions);
            if (m_LerpTarget == ScreensContainer.localPosition)
            {
                m_LerpTarget = m_Positions[0];
            }
        }
    }

    void FixedUpdate()
    {
        
            ScreensContainer.localPosition = Vector3.Lerp(ScreensContainer.localPosition, m_LerpTarget, 10 * Time.deltaTime);
            if (Vector3.Distance(ScreensContainer.localPosition, m_LerpTarget) < 0.001f)
            {
                m_Lerp = false;
            }
        
    }

    /// <summary>
    /// Bind t$$anonymous$$s to UnityEditor Event trigger Pointer Up
    /// </summary>
    public void DragEnd()
    {
        if (m_ScrollRect.horizontal)
        {
            m_Lerp = true;
            m_LerpTarget = FindClosestFrom(ScreensContainer.localPosition, m_Positions);
        }
    }

    /// <summary>
    /// Bind t$$anonymous$$s to UnityEditor Event trigger Drag
    /// </summary>
    public void OnDrag()
    {
        m_Lerp = false;
    }

    Vector3 FindClosestFrom(Vector3 start, List<Vector3> positions)
    {
        Vector3 closest = Vector3.zero;
        float distance = Mathf.Infinity;

        foreach (Vector3 position in m_Positions)
        {
            if (Vector3.Distance(start, position) < distance)
            {
                distance = Vector3.Distance(start, position);
                closest = position;
            }
        }

        return closest;
    }
    IEnumerator lerpTime(float time)
    {
        m_Lerp = true;
        yield return new WaitForSeconds(time);
        m_Lerp = false;
    }
}