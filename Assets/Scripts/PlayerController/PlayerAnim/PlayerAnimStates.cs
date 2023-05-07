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

    Vector2 _moveInputValue;
    Vector2 _aimInputValue;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _player = GetComponentInParent<PlayerController>();
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

        Vector2 _moveInputValue = _player.MoveInputValue;
        Vector2 _aimInputValue = _player.AimInputValue;

        Vector2 _normalizedMoveInputValue = _moveInputValue.normalized;
        Vector2 _normalizedAimInputValue = _aimInputValue.normalized;



        Vector2 _relativeVector = _normalizedMoveInputValue + _normalizedAimInputValue;

        //float _angle = Vector2.Angle(_normalizedAimInputValue, _normalizedMoveInputValue);

        

        float _deltaX = _normalizedMoveInputValue.x - _normalizedAimInputValue.x;
        float _deltaY = _normalizedMoveInputValue.y - _normalizedAimInputValue.y;

        float deg = Mathf.Atan2(_deltaY, _deltaX) * (180 / Mathf.PI);

        _relativeVector = Quaternion.Euler(deg, 0, deg) * Vector2.up;

        Debug.Log("MOVE: " + _normalizedMoveInputValue + "  AIM: " + _normalizedAimInputValue + "  ANGLE:" + deg);

        //Debug.DrawLine(transform.position, new Vector3(_normalizedMoveInputValue.x, 0,_normalizedMoveInputValue.y), Color.magenta);
        //Debug.DrawLine(transform.position, new Vector3(_normalizedAimInputValue.x, 0, _normalizedAimInputValue.y), Color.magenta);

        //Debug.Log(_angle);

        //_animator.SetFloat("right", _relativeVector.x);
        //_animator.SetFloat("forward", _relativeVector.y);

    }
}
