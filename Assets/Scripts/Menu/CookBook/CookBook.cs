using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CookBook : MonoBehaviour
{
    #region Variables

    [SerializeField]
    AnimationCurve m_showAnimPosCurve;

    [SerializeField]
    Image m_transition;

    [SerializeField] 
    List<PlayerCookingInventoryPage> m_inventoryPages;

    [SerializeField]
    RectTransform m_UIHolder;

    int _currentPageIndex = 0;

    static CookBook _instance;

    bool _areControlsLocked;
    Vector3 _shownPosition;
    Vector3 _hiddenPosition;
    float _curAnimProgress;
    Coroutine _curShowRoutine;

    public static CookBook Instance
    {
        get => _instance;
    }
    #endregion

    private void Awake()
    {
        //Set instance
        if(_instance == null)
        {
            _instance = this;
        }
        else if(_instance != this)
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        gameObject.SetActive(false);

        _shownPosition = m_UIHolder.localPosition;
        _hiddenPosition = _shownPosition;
        _hiddenPosition.y -= gameObject.GetComponent<RectTransform>().rect.height;
        m_UIHolder.localPosition = _hiddenPosition;
    }

    #region Visuals
    public void Show(bool value)
    {
        if (value)
        {
            gameObject.SetActive(true);

            if(_curShowRoutine != null)
            {
                StopCoroutine(_curShowRoutine);
            }
            _curShowRoutine = StartCoroutine(ShowAnimation());
        }
        else
        {
            if (_curShowRoutine != null)
            {
                StopCoroutine(_curShowRoutine);
            }
            _curShowRoutine = StartCoroutine(HideAnimation());
        }
    }

    IEnumerator ShowAnimation()
    {
        _areControlsLocked = true;
        Time.timeScale = 0f;

        float time = PlayerRuntimeData.GetInstance().data.CookData.ShowAnimDuration;
        Vector3 targetPos = _shownPosition;
        Vector3 initPos = m_UIHolder.localPosition;


        for (float f = _curAnimProgress > 0 ? (1 - _curAnimProgress) * time : 0; f < time; f += Time.unscaledDeltaTime)
        {
            _curAnimProgress = f / time;
            m_UIHolder.localPosition = Vector3.Lerp(initPos, targetPos, m_showAnimPosCurve.Evaluate(_curAnimProgress));
            m_UIHolder.localScale = Vector3.Lerp(Vector3.one * 0.5f, Vector3.one, m_showAnimPosCurve.Evaluate(_curAnimProgress));
            Time.timeScale = Mathf.Lerp(1, 0, m_showAnimPosCurve.Evaluate(_curAnimProgress));
            Color tempc = m_transition.color;
            tempc.a = Mathf.Lerp(0, 0.9f, m_showAnimPosCurve.Evaluate(_curAnimProgress));
            m_transition.color = tempc;
            yield return null;
        }

        m_UIHolder.localPosition = targetPos;
        _areControlsLocked = false;
        _curAnimProgress = 0;
        _curShowRoutine = null;
    }

    IEnumerator HideAnimation()
    {
        _areControlsLocked = true;
        Time.timeScale = 1f;

        float time = PlayerRuntimeData.GetInstance().data.CookData.ShowAnimDuration;
        Vector3 initPos = m_UIHolder.localPosition;
        Vector3 targetPos = _hiddenPosition;

        for (float f = _curAnimProgress > 0 ? (1 - _curAnimProgress) * time : 0; f < time; f += Time.unscaledDeltaTime)
        {
            _curAnimProgress = f / time;
            m_UIHolder.localPosition = Vector3.Lerp(targetPos, initPos, 1 - m_showAnimPosCurve.Evaluate(_curAnimProgress));
            m_UIHolder.localScale = Vector3.Lerp(Vector3.one * 0.5f, Vector3.one, 1 - m_showAnimPosCurve.Evaluate(_curAnimProgress));
            Time.timeScale = Mathf.Lerp(1, 0, 1 - m_showAnimPosCurve.Evaluate(_curAnimProgress));
            Color tempc = m_transition.color;
            tempc.a = Mathf.Lerp(0, 0.9f, 1 - m_showAnimPosCurve.Evaluate(_curAnimProgress));
            m_transition.color = tempc;
            yield return null;
        }

        m_UIHolder.localPosition = targetPos;
        _areControlsLocked = false;
        _curAnimProgress = 0;
        _curShowRoutine = null;
        gameObject.SetActive(false);
    }

    public void NextPage()
    {
        if (_areControlsLocked)
        {
            return;
        }

        //Multiple wheels check
        if (m_inventoryPages == null || m_inventoryPages.Count < 2)
        {
            Debug.Log("No inventory wheel to switch to");
            return;
        }

        //Check next wheel
        if (_currentPageIndex + 1 >= m_inventoryPages.Count)
        {
            return;
        }

        //Deactivate current wheel
        m_inventoryPages[_currentPageIndex].SetActive(false);

        //Increase index
        _currentPageIndex += 1;

        //Activate next wheel
        m_inventoryPages[_currentPageIndex].SetActive(true);
    }

    public void PrevPage()
    {
        if (_areControlsLocked)
        {
            return;
        }

        //Multiple wheels check
        if (m_inventoryPages == null || m_inventoryPages.Count < 2)
        {
            Debug.Log("No inventory wheel to switch to");
            return;
        }

        //Check prev wheel
        if (_currentPageIndex - 1 < 0)
        {
            return;
        }

        //Deactivate current wheel
        m_inventoryPages[_currentPageIndex].SetActive(false);

        //Increase index
        _currentPageIndex += -1;

        //Activate prev wheel
        m_inventoryPages[_currentPageIndex].SetActive(true);
    }
    #endregion

    #region Utility
    public bool IsDisplayed()
    {
        return gameObject.activeSelf;
    }
    #endregion

    [System.Serializable]
    private class PlayerCookingInventoryPage
    {
        [SerializeField]
        GameObject m_pageObject;

        public void SetActive(bool value)
        {
            m_pageObject.SetActive(value);
        }
    }
}


