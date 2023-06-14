using System;
using System.Collections;
using Enemy.Data;
using Enemy.Slime;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class TBH : EnemyController
{
    [SerializeField] private TBHData _data;
    [SerializeField] private GameObject _gun;
    [SerializeField] private GameObject _bullet;
    [SerializeField] private ParticleSystem m_stateSystem;
    [SerializeField] private CheckingSpawn _checkingSpawn;

    [SerializeField] private Animator _animator;
    
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
        _animator = GetComponentInChildren<Animator>();
        Healthpoint = _data.Health;
        _checkingSpawn = GetComponentInChildren<CheckingSpawn>();
    }
    
    private void Reset()
    {
        Healthpoint = _data.Health;
    }

    private void FixedUpdate()
    {
        if (_state != State.Dying) 
        {
            AreaDetection();
        }
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

                _animator.SetBool("isTeleport", false);

                StartCoroutine(Waiting());

                IEnumerator Waiting() 
                {
                    yield return new WaitForSeconds(1f);
                    _animator.SetBool("isShoot", true);
                    Attack(Shot, _data.AttackSpeed);
                }
                break;
            case State.Teleporting:
                //_animator.SetBool("isShoot", false);
                Teleport();
                break;
            case State.Casting:
                //_animator.SetBool("isShoot", false);
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
        
        transform.LookAt(new Vector3(Player.transform.position.x, transform.position.y, Player.transform.position.z));

        for (int i = 0; i < _data.NumberOfBullet; i++)
        {
            StartCoroutine(IShotRandomly(i));
        }

        SetState(State.Casting);

        IEnumerator IShotRandomly(int i)
        {
            var spread = Random.Range(-2f, 2f);
            
            Quaternion rotation = quaternion.Euler(angleStep * i, 0, angleStep * i);
            Vector3 direction = rotation * transform.forward;
            
            float randomDelay = Random.Range(0.1f, 0.5f);
            yield return new WaitForSeconds(randomDelay);
            
            GameObject bullet = Instantiate(_bullet, _gun.transform.position, Quaternion.identity);

            bullet.GetComponent<EnemyBulletController>().SetDirection(new Vector3(Player.transform.position.x + spread, Player.transform.position.y, Player.transform.position.z + spread));
            bullet.GetComponent<EnemyBulletController>().SetDamage(_data.DamagePerBullet);
        }
    }

    private void Teleport()
    {

        Vector3 randomPosition = UnityEngine.Random.insideUnitSphere.normalized * _data.AttackRange + Player.transform.position;
        
        if (Vector3.Distance(transform.position, randomPosition) <= _data.MinimumRadius)
            randomPosition = UnityEngine.Random.insideUnitSphere * _data.AttackRange + Player.transform.position;

        Vector3 position = new Vector3(randomPosition.x, transform.position.y, randomPosition.z);
        
        if (CanTeleportHere(position))
            transform.position = new Vector3(randomPosition.x, transform.position.y, randomPosition.z);
        else
            Teleport();

        
        SetState(State.Attacking);
    }

    private bool CanTeleportHere(Vector3 position)
    {
        RaycastHit hit;
        Vector3 direction = Player.transform.position - position;
        
        if (Physics.Raycast(position, direction, out hit, _data.AttackRange))
        {
            if (hit.collider.CompareTag("Player"))
                return true;
        }

        return false;
    }

    private void Casting()
    {
        _animator.SetBool("isTeleport", true);
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

    protected override void Dying()
    {
        StopAllCoroutines();

        _animator.SetBool("isDead", true);

        StartCoroutine(IDeathAnim());

        IEnumerator IDeathAnim()
        {
            yield return new WaitForSeconds(2f);
            base.Dying();
        }
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
