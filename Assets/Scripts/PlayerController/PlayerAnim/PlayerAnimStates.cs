using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimStates : MonoBehaviour
{

    public enum playerAnimStates
    {
        IDLE,
        IDLEATTACK,
        RUNNING,
        RUNNINGATTACK
    }

    public playerAnimStates animStates;

    private Animator _animator;

    [SerializeField]
    private Transform Marmite;
    [SerializeField]
    private Transform aimedMarmite;

    private PlayerController _player;    
    private PlayerAttack _playerAttack;

    Vector2 _moveInputValue;
    Vector2 _aimInputValue;
    float _moveInputMagnitude;

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
                    AimedMarmite(true);
                    break;
                }
            case playerAnimStates.RUNNING:
                {
                    _animator.SetBool("idleAttackBool", false);
                    _animator.SetBool("runningBool", true);
                    _animator.SetBool("runningAttackBool", false);
                    AimedMarmite(false);
                    break;
                }
            case playerAnimStates.RUNNINGATTACK:
                {
                    _animator.SetBool("idleAttackBool", false);
                    _animator.SetBool("runningBool", true);
                    _animator.SetBool("runningAttackBool", true);
                    AimedMarmite(true);
                    break;
                }
            default: 
                {
                    _animator.SetBool("idleAttackBool", false);
                    _animator.SetBool("runningBool", false);
                    _animator.SetBool("runningAttackBool", false);
                    AimedMarmite(false);
                    break;
                }
        }


    //        public bool ShootOnCooldown
    //{
    //    get => _shootOnCooldown;
    //}
    //if (_playerAttack.ShootOnCooldown)
    //{
    //    _animator.SetBool("shoot", true);
    //}
    //else
    //{
    //    _animator.SetBool("shoot", false);
    //}
}

    private void AimedMarmite(bool isAimed) 
    {
        if (!isAimed)
        {
            Marmite.gameObject.SetActive(true);
            aimedMarmite.gameObject.SetActive(false);
        }
        else 
        {
            Marmite.gameObject.SetActive(false);
            aimedMarmite.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    public void FixedUpdate()
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
}
