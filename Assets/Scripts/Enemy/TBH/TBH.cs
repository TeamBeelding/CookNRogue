using System;
using System.Collections;
using System.Collections.Generic;
using Enemy.Data;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class TBH : EnemyController
{
    [SerializeField] private TBHData _data;
    [SerializeField] private GameObject _gun;
    [SerializeField] private GameObject _bullet;
    [SerializeField] private ParticleSystem m_stateSystem;
    
    public enum State
    {
        Neutral,
        Attacking,
        Teleporting,
        Casting,
        Dying
    }
    
    public State _state;

    protected override void Awake()
    {
        base.Awake();
        
        Healthpoint = _data.Health;
    }
    
    private void Reset()
    {
        Healthpoint = _data.Health;
    }

    private void FixedUpdate()
    {
        AreaDetection();
    }

    private void AreaDetection()
    {
        if (_state != State.Neutral)
            return;
        
        if (Vector3.Distance(transform.position, Player.transform.position) <= _data.AttackRange)
        {
            SetState(State.Attacking);
        }
    }

    private void StateManagement()
    {
        switch (_state)
        {
            case State.Neutral:
                break;
            case State.Attacking:
                Attack(Shot, _data.AttackSpeed);
                break;
            case State.Teleporting:
                Teleport();
                break;
            case State.Casting:
                Casting();
                break;
            case State.Dying:
                Dying();
                break;
            default:
                Dying();
                break;
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void Shot()
    {
        float angleStep = _data.AngleShot / _data.NumberOfBullet;
        
        transform.LookAt(Player.transform);

        for (int i = 0; i < _data.NumberOfBullet; i++)
        {
            var spread = Random.Range(-2f, 2f);
            
            Quaternion rotation = quaternion.Euler(angleStep * i, 0, angleStep * i);
            Vector3 direction = rotation * transform.forward;
            
            GameObject bullet = Instantiate(_bullet, _gun.transform.position, Quaternion.identity);
            
            bullet.GetComponent<EnemyBulletController>().SetDirection(new Vector3(Player.transform.position.x + spread, Player.transform.position.y, Player.transform.position.z + spread));
            bullet.GetComponent<EnemyBulletController>().SetDamage(_data.DamagePerBullet);
        }

        SetState(State.Casting);
    }

    private void Teleport()
    {
        Vector3 randomPosition = UnityEngine.Random.insideUnitSphere.normalized * _data.AttackRange + Player.transform.position;
        
        if (Vector3.Distance(transform.position, randomPosition) <= _data.MinimumRadius)
            randomPosition = UnityEngine.Random.insideUnitSphere * _data.AttackRange + Player.transform.position;
        
        transform.position = new Vector3(randomPosition.x, transform.position.y, randomPosition.z);
        
        SetState(State.Attacking);
    }

    private void Casting()
    {
        StartCoroutine(Waiting());
        
        IEnumerator Waiting()
        {
            yield return new WaitForSeconds(_data.DelayBetweenTeleport);
            SetState(State.Teleporting);
        }
    }

    private void SetState(State value)
    {
        _state = value;

        Debug.Log(_state);
        
        StateManagement();
    }

    public State GetState()
    {
        return _state;
    }

    public override bool IsMoving()
    {
        return false;
    }

    public override void TakeDamage(float damage = 1, bool isCritical = false)
    {
        base.TakeDamage(damage, isCritical);

        if (Healthpoint <= 0)
            SetState(State.Dying);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _data.AttackRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _data.TeleportRange);
    }
}
