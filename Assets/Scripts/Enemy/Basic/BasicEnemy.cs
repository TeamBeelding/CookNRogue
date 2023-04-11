using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BasicEnemy : EnemyController
{
    public enum State
    {
        Neutral,
        Chase,
        Attack,
        Dying,
    }

    public State state;
    
    protected override void Awake()
    {
        base.Awake();
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
        IStateManagement();
    }

    private void FixedUpdate()
    {
        if (state == State.Dying)
            return;
        
        AreaDetection();
    }
    
    public State GetState()
    {
        return state;
    }
    
    public void SetState(State value)
    {
        state = value;
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
            case State.Attack:
                Attack();
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
        
        if (Vector3.Distance(transform.position, player.transform.position) <= data.GetRangeDetection())
        {
            if (Vector3.Distance(transform.position, player.transform.position) <= data.GetAttackSpeed())
            {
                state = State.Attack;
            }
            else
            {
                state = State.Chase;
            }
        }
    }

    protected override void Chase()
    {
        if (state == State.Dying)
            return;
        
        base.Chase();
    }

    protected override void Attack()
    {
        if (state == State.Dying)
            return;
        
        base.Attack();
    }

    protected override void Dying()
    {
        base.Dying();
    }
    
    public override bool IsMoving()
    {
        return state == State.Chase;
    }

    public override void TakeDamage(float damage = 1, bool isCritical = false)
    {
        base.TakeDamage();
        
        if (state == State.Neutral)
            state = State.Chase;
        
        if (healthpoint <= 0)
        {
            state = State.Dying;
        }
    }
}