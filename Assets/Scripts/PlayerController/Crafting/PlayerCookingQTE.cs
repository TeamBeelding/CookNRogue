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
    
    private PlayerController _playerController;

    bool _isActive;

    Coroutine _curLoop;

    private void Reset()
    {
        m_playerCookingScript = m_playerCookingScript != null ? m_playerCookingScript : GetComponent<PlayerCooking>();

        m_QTEDuration = new Vector2(1f, 2f);
    }

    private void Start()
    {
        _playerController = GetComponent<PlayerController>();
        m_playerCookingScript = m_playerCookingScript != null ? m_playerCookingScript : GetComponent<PlayerCooking>();
        m_QTEVisuals.SetActive(false);
    }

    public void StartQTE(float delay)
    {
        float randDuration = Random.Range(m_QTEDuration.x, m_QTEDuration.y);
        _curLoop = StartCoroutine(QTELoop(delay, randDuration));
    }

    public void FailQTE()
    {
        if (_curLoop == null)
            return;

        _isActive = false;
        m_QTEVisuals.SetActive(false);
        
        _playerController.QTEFailed();

        StopCoroutine(_curLoop);
        _curLoop = null;
    }

    private void CompleteQTE()
    {
        if (_curLoop == null)
            return;

        m_playerCookingScript.CompletedQTE();

        _isActive = false;
        m_QTEVisuals.SetActive(false);

        StopCoroutine(_curLoop);
        _curLoop = null;
    }

    //Called by input manager
    public void CheckQTE()
    {
        if(_curLoop != null)
        {
            if (_isActive)
            {
                CompleteQTE();
            }
            else
            {
                FailQTE();
            }
        }
    }

    private IEnumerator QTELoop(float delay, float duration)
    {
        yield return new WaitForSeconds(delay);
        _isActive = true;
        m_QTEVisuals.SetActive(true);
        _playerController.QTEAppear();

        yield return new WaitForSeconds(duration);
        FailQTE();
    }
}
