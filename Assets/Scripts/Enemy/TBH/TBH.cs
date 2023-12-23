using System.Collections;
using Enemy.Data;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class TBH : EnemyController
{
    [Header("Sound")]
    [SerializeField]
    private AK.Wwise.Event _Play_SFX_Carrot_Attack;
    [SerializeField]
    private AK.Wwise.Event _Play_SFX_Carrot_Hit;
    [SerializeField]
    private AK.Wwise.Event _Play_SFX_Carrot_Death;
    [SerializeField]
    private AK.Wwise.Event _Play_Weapon_Hit;

    [SerializeField] private TBHData _data;
    [SerializeField] private GameObject _gun;
    [SerializeField] private GameObject _bullet;
    [SerializeField] private ParticleSystem m_stateSystem;

    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject physics;

    int walkableMask = 1;

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
        walkableMask = 1 << NavMesh.GetAreaFromName("Walkable");
        base.Awake();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        _animator = GetComponentInChildren<Animator>();
        Healthpoint = _data.Health;

        physics.SetActive(true);
        _collider.enabled = true;
    }

    private void Reset()
    {
        Healthpoint = _data.Health;
    }

    private void FixedUpdate()
    {
        if (_state != State.Dying) 
            AreaDetection();
    }

    private void AreaDetection()
    {
        if (_state != State.Neutral)
            return;
        
        if (Vector3.Distance(transform.position, Player.transform.position) <= _data.AttackRange)
            SetState(State.Attacking);
    }

    private void StateManagement()
    {
        switch (_state)
        {
            case State.Neutral:
                break;
            case State.Attacking:

                //_Play_SFX_Carrot_Erupt.Post(gameObject);
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
                Teleport();
                break;
            case State.Casting:
                Casting();
                break;
            case State.Dying:
                _Play_SFX_Carrot_Death.Post(gameObject);
                Dying();
                break;
            default:
                _Play_SFX_Carrot_Death.Post(gameObject);
                Dying();
                break;
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void Shot()
    {
        //float angleStep = _data.AngleShot / _data.NumberOfBullet;
        
        transform.LookAt(new Vector3(Player.transform.position.x, transform.position.y, Player.transform.position.z));

        for (int i = 0; i < _data.NumberOfBullet; i++)
        {
            StartCoroutine(IShotRandomly());
        }

        SetState(State.Casting);

        IEnumerator IShotRandomly()
        {
            float spread = Random.Range(_data.GetMinSpread, _data.GetMaxSpread);
            float randomDelay = Random.Range(_data.GetMinSpeed, _data.GetMaxSpeed);

            yield return new WaitForSeconds(randomDelay);
            
            GameObject bullet = PoolManager.Instance.InstantiateFromPool(PoolType.Bullet, _gun.transform.position, Quaternion.identity);

            bullet.GetComponent<EnemyBulletController>().SetDirection(new Vector3(Player.transform.position.x + spread, Player.transform.position.y, Player.transform.position.z + spread));
            bullet.GetComponent<EnemyBulletController>().SetDamage(_data.DamagePerBullet);
            _Play_SFX_Carrot_Attack.Post(bullet);
        }
    }

    private void Teleport()
    {
        NavMeshHit hit;
        bool validPositionFound = false;

        while (!validPositionFound)
        {
            Vector3 randomPoint = transform.position + Random.insideUnitSphere.normalized * _data.AttackRange;

            if (NavMesh.SamplePosition(randomPoint, out hit, _data.AttackRange, walkableMask))
            {
                Vector3 position = hit.position;

                if (Vector3.Distance(transform.position, position) <= _data.MinimumRadius)
                    continue;

                if (CanTeleportHere(position))
                {
                    transform.position = new Vector3(position.x, transform.position.y, position.z);
                    validPositionFound = true;
                }
            }
        }

        SetState(State.Attacking);
    }

    private bool CanTeleportHere(Vector3 position)
    {
        RaycastHit hit;
        Vector3 direction = Player.transform.position - position;

        Vector3 origin = position - (direction.normalized * 10);

        if (Physics.Raycast(origin, direction, out hit, _data.AttackRange))
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

    public State GetState() => _state;

    public override bool IsMoving()
    {
        return false;
    }

    protected override void Dying()
    {
        physics.SetActive(false);
        StopAllCoroutines();

        _animator.SetBool("isDead", true);

        if (gameObject.activeSelf)
            StartCoroutine(IDeathAnim());

        IEnumerator IDeathAnim()
        {
            yield return new WaitForSeconds(2f);
            base.Dying();
        }
    }

    public override void TakeDamage(float damage = 1, bool isCritical = false)
    {
        if (GetState() == State.Dying)
            return;

        base.TakeDamage(damage, isCritical);
        _Play_SFX_Carrot_Hit.Post(gameObject);
        _Play_Weapon_Hit.Post(gameObject);

        if (Healthpoint <= 0)
        {
            waveManager.SlowMotion();
            hasAskForSlow = true;
            SetState(State.Dying);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _data.AttackRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _data.TeleportRange);
    }
#endif
}
