using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class BasicEnemy : EnemyController
{
    [SerializeField]
    private EnemyData data;
    [SerializeField]
    private NavMeshAgent _agent;
    
    [SerializeField]
    private GameObject m_gun;
    [SerializeField]
    private GameObject m_bullet;
    [SerializeField]
    private ParticleSystem m_stateSystem;
    [SerializeField]
    private Renderer stateRenderer;

    public enum State
    {
        Neutral,
        Chase,
        Attack,
        Dying,
    }

    [SerializeField]
    private State state;
    
    protected override void Awake()
    {
        base.Awake();
        
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = data.GetSpeed();
        _agent.stoppingDistance = data.GetAttackRange();
        _focusPlayer = data.GetFocusPlayer();
        healthpoint = data.GetHealth();
        
        stateRenderer = m_stateSystem.GetComponent<Renderer>();
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
                Attack(Shot, data.GetAttackSpeed());
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

        _agent.SetDestination(player.transform.position);
        
        stateRenderer.material.color = Color.yellow;
        m_stateSystem.gameObject.SetActive(true);
    }
    
    protected override void Attack(UnityAction Shot, float delay)
    {
        if (state == State.Dying)
            return;
        
        stateRenderer.material.color = Color.red;
        m_stateSystem.gameObject.SetActive(true);

        base.Attack(Shot);
    }

    private void Shot()
    {
        GameObject shot = Instantiate(m_bullet, m_gun.transform.position, Quaternion.identity);
        shot.GetComponent<EnemyBulletController>().SetDirection(player.transform);
    }
    
    private new void Dying()
    {
        base.Dying();
        m_stateSystem.gameObject.SetActive(false);
    }

    public override bool IsMoving()
    {
        return state == State.Chase;
    }

    public override void TakeDamage(float damage = 1, bool isCritical = false)
    {
        base.TakeDamage(damage, isCritical);
        
        if (state == State.Neutral)
            state = State.Chase;
        
        if (healthpoint <= 0)
        {
            state = State.Dying;
        }
    }
    
    #if UNITY_EDITOR
    
    protected override void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, data.GetRangeDetection());
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, data.GetAttackRange());
    }
    
    #endif
}