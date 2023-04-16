using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class MinimoyzController : EnemyController
{
    [SerializeField] private MinimoyzData _data;
    [SerializeField] private NavMeshAgent _agent;

    private Coroutine _castingCoroutine;
    private Coroutine _attackCoroutine;
    private bool shouldChaseAndAttack;
    
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
        
        StateManagement();
    }

    protected void FixedUpdate()
    {
        if (state == State.Dying)
            return;
        
        AreaDetection();
    }

    public void SetFocus(bool value = true)
    {
        _focusPlayer = value;
    }

    private void StateManagement()
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
            
            if (shouldChaseAndAttack)
                state = State.ChaseAndAttack;
            else
                state = State.Chase;
        }
        else
        {
            state = State.Neutral;
        }

        if (Vector3.Distance(transform.position, player.transform.position) <= _data.GetAttackRange())
        {
            if (shouldChaseAndAttack)
                state = State.ChaseAndAttack;
            else
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
        
        if (_castingCoroutine == null)
        {
            _castingCoroutine = StartCoroutine(CastAttack());
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
        
        if (_attackCoroutine == null)
            _attackCoroutine = StartCoroutine(ISettingAttack());
        
        IEnumerator ISettingAttack()
        {
            yield return new WaitForSeconds(0.4f);

            if (Vector3.Distance(transform.position, player.transform.position) > _data.GetAttackRange())
            {
                CancelCast();
                _attackCoroutine = null;
                SetState(State.Chase);
            }

            yield return new WaitForSeconds(0.5f);
            
            if (Vector3.Distance(transform.position, player.transform.position) > _data.GetAttackRange())
            {
                shouldChaseAndAttack = true;
                _attackCoroutine = null;
                SetState(State.ChaseAndAttack);
            }
            else
            {
                _attackCoroutine = null;
                HitPlayer();
            }
        }
    }
    
    private void CancelCast()
    {
        _castingCoroutine = null;
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
        {
            HitPlayer();
            shouldChaseAndAttack = false;
        }
    }
    
    public override void TakeDamage(float damage = 1, bool isCritical = false)
    {
        base.TakeDamage(damage, isCritical);
        
        if (healthpoint <= 0)
        {
            state = State.Dying;
        }
    }
}
