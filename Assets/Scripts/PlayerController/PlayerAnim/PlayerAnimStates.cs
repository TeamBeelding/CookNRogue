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

    private bool _runningAttack = false;

    private Animator _animator;

    [SerializeField]
    private Transform Marmite;
    [SerializeField]
    private Transform aimedMarmite;

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

}
