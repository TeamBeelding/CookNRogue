using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class Slime : EnemyController
{
    [SerializeField] private SlimeData _data;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField, Required("Prefabs minimoyz")] private GameObject _minimoyz;
    
    public enum State
    {
        Neutral,
        KeepingDistance,
        Chase,
        Attack,
        Dying,
    }
    
    public State state;

    private void Reset()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    protected override void Awake()
    {
        base.Awake();
        
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = _data.GetSpeed;
        _agent.stoppingDistance = _data.GetAttackRange;
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
        StateManagement();
    }

    private void FixedUpdate()
    {
        if (state == State.Dying)
            return;
        
        AreaDetection();
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void StateManagement()
    {
        switch (state)
        {
            case State.Neutral:
                break;
            case State.Chase:
                Chase();
                break;
            case State.KeepingDistance:
                KeepDistance();
                break;
            case State.Attack:
                Attack(ThrowMinimoyz, _data.GetAttackSpeed);
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
        
        if (Vector3.Distance(transform.position, player.transform.position) <= _data.GetFocusRange)
        {
            _focusPlayer = true;
            state = State.Chase;
        }
        else
        {
            state = State.Neutral;
        }

        if (Vector3.Distance(transform.position, player.transform.position) <= _data.GetAttackRange)
        {
            if (Vector3.Distance(transform.position, player.transform.position) <= _data.GetMinimumDistanceToKeep)
                state = State.KeepingDistance;
            else
                state = State.Attack;
        }
        else
        {
            if (_focusPlayer)
                state = State.Chase;
        }
    }
    
    private void KeepDistance()
    {
        if (Vector3.Distance(transform.position, player.transform.position) <= _data.GetMinimumDistanceToKeep)
        {
            Vector3 target = transform.position - (player.transform.position - transform.position);
            _agent.stoppingDistance = 0;
            _agent.SetDestination(target);
        }
    }
    
    protected override void Chase()
    {
        _agent.stoppingDistance = _data.GetAttackRange;
        _agent.SetDestination(player.transform.position);
    }

    private void ThrowMinimoyz()
    {
        Debug.Log("Throw Minimoyz");
    }

    protected override void Dying()
    {
        for (int i = 0; i < _data.GetSlimeSpawnWhenDying; i++)
        {
            Vector2 origin = new Vector2(transform.position.x, transform.position.z);
            Vector3 point = Random.insideUnitCircle * _data.GetRadiusMinimoyzSpawnPoint + origin;
            point = new Vector3(point.x, 0, point.y);
            
            Instantiate(_minimoyz, point, Quaternion.identity);
        }
        
        base.Dying();
    }

    public override bool IsMoving()
    {
        throw new System.NotImplementedException();
    }
}
