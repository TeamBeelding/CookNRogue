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

    private float _horizontalAxis;
    private float _verticalAxis;

    public Transform cam;
    Vector3 camForward;
    Vector3 move;
    Vector3 moveInput;

    float forwardAmount;
    float strafeAmount;

    void Start()
    {
        _animator = GetComponent<Animator>();
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
    void FixedUpdate()
    {
        _horizontalAxis = Input.GetAxis("Horizontal");
        _verticalAxis = Input.GetAxis("Vertical");

        //Vector2 _moveInputValue;
        //Vector2 _aimInputValue;

        move = _verticalAxis * Vector3.forward + _horizontalAxis * Vector3.right;
        //}

        //if (move.magnitude > 1)
        //{
        //    move.Normalize();
        //}

        Move(move);

        //_animator.SetFloat("forward", _verticalAxis, 0.1f, Time.deltaTime);
        //_animator.SetFloat("strafe", _horizontalAxis, 0.1f, Time.deltaTime);

    }

    void Move(Vector3 currentMove)
    {
        if (currentMove.magnitude > 1)
        {
            currentMove.Normalize();
        }
        moveInput = currentMove;

        ConvertMoveInput();
        UpdateAnimator();
    }

    void ConvertMoveInput()
    {
        Vector3 localMove = transform.InverseTransformDirection(moveInput);
        strafeAmount = localMove.x;
        forwardAmount = localMove.z;
    }
    void UpdateAnimator()
    {
        _animator.SetFloat("forward", forwardAmount, 0.1f, Time.deltaTime);
        _animator.SetFloat("strafe", strafeAmount, 0.1f, Time.deltaTime);
    }

}
