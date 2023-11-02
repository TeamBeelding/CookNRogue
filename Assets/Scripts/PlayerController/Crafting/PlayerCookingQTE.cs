using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerCooking))]
public class PlayerCookingQTE : MonoBehaviour
{
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

        PlayerRuntimeData.GetInstance().data.CookData.QteDuration = PlayerRuntimeData.GetInstance().data.CookData.DefaultQteDuration;
    }

    private void Start()
    {
        _playerController = GetComponent<PlayerController>();
        m_playerCookingScript = m_playerCookingScript != null ? m_playerCookingScript : GetComponent<PlayerCooking>();
        m_QTEVisuals.SetActive(false);
        PlayerRuntimeData.GetInstance().data.CookData.QteDuration = PlayerRuntimeData.GetInstance().data.CookData.DefaultQteDuration;
    }

    public void StartQTE(float delay)
    {
        float randDuration = Random.Range(PlayerRuntimeData.GetInstance().data.CookData.QteDuration.x, PlayerRuntimeData.GetInstance().data.CookData.QteDuration.y);
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

        if (_curLoop != null)
            StopCoroutine(_curLoop);
        
        _curLoop = null;
    }

    public void ResetQTE()
    {
        _isActive = false;
        m_QTEVisuals.SetActive(false);

        if (_curLoop != null)
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
