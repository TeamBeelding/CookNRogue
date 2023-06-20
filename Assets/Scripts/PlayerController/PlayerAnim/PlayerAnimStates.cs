using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimStates : MonoBehaviour
{
    
    public enum playerAnimStates
    {
        IDLE,
        //COOKING,
        IDLEATTACK,
        RUNNING,
        RUNNINGATTACK
    }

    public playerAnimStates animStates;

    public Animator _animator;
    //public Animator Animator 
    //{ 
    //    get { return _animator; }
    //}

    [SerializeField]
    private Transform m_marmite;
    [SerializeField]
    private Transform m_aimedMarmite;
    [SerializeField]
    private Transform m_cookingMarmite;

    [SerializeField]
    private Transform m_spoon;
    [SerializeField]
    private Transform m_aimedSpoon;
    [SerializeField]
    private Transform m_cookingSpoon;

    private PlayerController _player;    
    private PlayerAttack _playerAttack;

    Vector2 _moveInputValue;
    Vector2 _aimInputValue;
    float _moveInputMagnitude;

    [SerializeField]
    private AK.Wwise.Event _Play_MC_Run_Footstep;
    [SerializeField]
    private AK.Wwise.Event _Play_MC_Run_Foley;
    [SerializeField]
    private AK.Wwise.Event _Play_MC_Run;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _player = GetComponentInParent<PlayerController>();
        _playerAttack = GetComponentInParent<PlayerAttack>();
    }

    private void Update()
    {
        switch (animStates)
        {
            case playerAnimStates.IDLEATTACK:
                {
                    _animator.SetBool("idleAttackBool", true);
                    _animator.SetBool("runningBool", false);
                    _animator.SetBool("runningAttackBool", false);
                    Marmite(true, false);
                    break;
                }
            case playerAnimStates.RUNNING:
                {
                    _animator.SetBool("idleAttackBool", false);
                    _animator.SetBool("runningBool", true);
                    _animator.SetBool("runningAttackBool", false);
                    Marmite(false, false);
                    break;
                }
            case playerAnimStates.RUNNINGATTACK:
                {
                    _animator.SetBool("idleAttackBool", false);
                    _animator.SetBool("runningBool", true);
                    _animator.SetBool("runningAttackBool", true);
                    Marmite(true, false);
                    break;
                }
            default: 
                {
                    _animator.SetBool("idleAttackBool", false);
                    _animator.SetBool("runningBool", false);
                    _animator.SetBool("runningAttackBool", false);
                    Marmite(false, false);
                    break;
                }
        }

        if (_playerAttack.ShootOnCooldown)
        {
            _animator.SetBool("shoot", true);
        }
        else
        {
            _animator.SetBool("shoot", false);
        }

        if (_player.IsDashing && !_animator.GetBool("dash"))
        {
            _animator.SetBool("dash", true);
        }
        else if (!_player.IsDashing)
        {
            _animator.SetBool("dash", false);
        }
    }

    public void Marmite(bool isAimed, bool isCooking) 
    {
        if (isAimed)
        {
            Debug.Log("isAimed was called");
            m_marmite.gameObject.SetActive(false);
            m_aimedMarmite.gameObject.SetActive(true);
            m_cookingMarmite.gameObject.SetActive(false);
            m_spoon.gameObject.SetActive(false);
            m_aimedSpoon.gameObject.SetActive(true);
            m_cookingMarmite.gameObject.SetActive(false);
        }
        else if (isCooking) 
        {
            Debug.Log("isCooking was called");
            m_marmite.gameObject.SetActive(false);
            m_aimedMarmite.gameObject.SetActive(false);
            m_cookingMarmite.gameObject.SetActive(true);
            m_spoon.gameObject.SetActive(false);
            m_aimedSpoon.gameObject.SetActive(false);
            m_cookingSpoon.gameObject.SetActive(true);
        }
        else
        {
            m_marmite.gameObject.SetActive(true);
            m_aimedMarmite.gameObject.SetActive(false);
            m_cookingMarmite.gameObject.SetActive(false);
            m_spoon.gameObject.SetActive(true);
            m_aimedSpoon.gameObject.SetActive(false);
            m_cookingSpoon.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        _moveInputValue = _player.MoveInputValue;
        _moveInputMagnitude = _player.MoveInputValue.magnitude;

        _aimInputValue = _player.AimInputValue;

        Vector2 _normalizedMoveInputValue = _moveInputValue.normalized;
        Vector2 _normalizedAimInputValue = _aimInputValue.normalized;

        float _angle = Vector2.SignedAngle(_normalizedAimInputValue, _normalizedMoveInputValue);

        Vector2 dir = Vector2.zero;
        dir.x = Mathf.Cos(_angle * Mathf.Deg2Rad);
        dir.y = Mathf.Sin(_angle * Mathf.Deg2Rad);

        _animator.SetFloat("right", _moveInputMagnitude * dir.x);
        _animator.SetFloat("forward", _moveInputMagnitude * dir.y);
    }

    public void PlayerFootstep()
    {
        _Play_MC_Run.Post(gameObject);
    }
}
