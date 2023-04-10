using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class MinimoyzController : EnemyController
{
    [SerializeField] private MinimoyzData _data;
    [SerializeField] private NavMeshAgent _agent;
    
    private Coroutine castingCoroutine;
    private Coroutine attackCoroutine;
    
    public enum State
    {
        Neutral,
        Chase,
        Cast,
        Attack,
        ChaseAndAttack,
        Dying,
    }
    
    public State state;

    protected override void Awake()
    {
        base.Awake();
        
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = _data.GetSpeed();
        _agent.stoppingDistance = _data.GetAttackRange();
        _focusPlayer = _data.GetFocusPlayer();
        healthpoint = _data.GetHealth();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        state = _focusPlayer ? State.Chase : State.Neutral;


        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        
        IStateManagement();
    }

    protected void FixedUpdate()
    {
        if (state == State.Dying)
            return;
        
        AreaDetection();
    }

    private void IStateManagement()
    {
        switch (state)
        {
            case State.Neutral:
                break;
            case State.Chase:
                Chase();
                break;
            case State.Cast:
                Cast();
                break;
            case State.Attack:
                Attack(Attack, _data.GetAttackSpeed());
                break;
            case State.ChaseAndAttack:
                ChaseAndAttack();
                break;
            case State.Dying:
                Dying();
                break;
            default:
                Dying();
                break;
        }
    }
    
    private void AreaDetection()
    {
        if (state == State.Dying)
            return;

        if (Vector3.Distance(transform.position, player.transform.position) <= _data.GetFocusRange())
        {
            _focusPlayer = true;
            state = State.Chase;
        }
        else
        {
            state = State.Neutral;
        }

        if (Vector3.Distance(transform.position, player.transform.position) <= _data.GetAttackRange())
        {
            state = State.Attack;
        }
        else
        {
            if (_focusPlayer)
                state = State.Chase;
        }
    }
    
    public State GetState()
    {
        return state;
    }
    
    public void SetState(State value)
    {
        state = value;
    }

    public override bool IsMoving()
    {
        throw new System.NotImplementedException();
    }
    
    private void Cast()
    {
        if (state == State.Dying)
            return;
        
        if (castingCoroutine == null)
        {
            castingCoroutine = StartCoroutine(CastAttack());
        }
    }

    private IEnumerator CastAttack()
    {
        yield return new WaitForSeconds(0.5f);
    }
    
    private void Attack()
    {
        if (state == State.Dying)
            return;
        
        if (attackCoroutine == null)
            attackCoroutine = StartCoroutine(ISettingAttack());
        
        HitPlayer();

        IEnumerator ISettingAttack()
        {
            yield return new WaitForSeconds(0.4f);

            if (Vector3.Distance(transform.position, player.transform.position) > _data.GetAttackRange())
            {
                CancelCast();
                SetState(State.Chase);
            }

            yield return new WaitForSeconds(0.5f);

            if (Vector3.Distance(transform.position, player.transform.position) > _data.GetAttackRange())
            {
                SetState(State.ChaseAndAttack);
            }
        }
    }
    
    private void CancelCast()
    {
        castingCoroutine = null;
        state = State.Chase;
    }

    private void HitPlayer()
    {
        player.GetComponent<PlayerController>().TakeDamage(_data.GetDamage());
        SetState(State.Cast);
    }

    protected override void Chase()
    {
        _agent.SetDestination(player.transform.position);
    }
    
    private void ChaseAndAttack()
    {
        Chase();
        
        if (Vector3.Distance(transform.position, player.transform.position) <= _data.GetAttackRange())
            HitPlayer();
    }
}
