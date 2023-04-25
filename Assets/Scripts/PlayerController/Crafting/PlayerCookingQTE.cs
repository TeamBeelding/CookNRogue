using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerCooking))]
public class PlayerCookingQTE : MonoBehaviour
{
    [SerializeField, Tooltip("random ammount of time in seconds available to validate the QTE," +
        " where x is the lowest possible value and y is the highest")]
    Vector2 m_QTEDuration;

    [SerializeField]
    PlayerCooking m_playerCookingScript;

    [SerializeField]
    GameObject m_QTEVisuals;

    float _progress;
    bool _isActive;

    Coroutine _curLoop;

    private void Reset()
    {
        m_playerCookingScript = m_playerCookingScript != null ? m_playerCookingScript : GetComponent<PlayerCooking>();

        m_QTEDuration = new Vector2(1f, 2f);
    }

    private void Start()
    {
        m_playerCookingScript = m_playerCookingScript != null ? m_playerCookingScript : GetComponent<PlayerCooking>();
        m_QTEVisuals.SetActive(false);
    }

    public void StartQTE()
    {
        _isActive = true;
        m_QTEVisuals.SetActive(true);

        float randDuration = Random.Range(m_QTEDuration.x, m_QTEDuration.y);
        _curLoop = StartCoroutine(QTELoop(randDuration));
    }

    public void FailQTE()
    {
        _isActive = false;
        m_QTEVisuals.SetActive(false);
    }

    private void CompleteQTE()
    {
        m_playerCookingScript.CompletedQTE();

        _isActive = false;
        m_QTEVisuals.SetActive(false);
    }

    //Called by input manager
    public void CheckQTE()
    {
        if (_isActive)
        {
            CompleteQTE();
            StopCoroutine(_curLoop);
        }
    }

    private IEnumerator QTELoop(float duration)
    {
        float tProgress = 0f;
        while(tProgress < duration)
        {
            tProgress += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        FailQTE();
    }
}
