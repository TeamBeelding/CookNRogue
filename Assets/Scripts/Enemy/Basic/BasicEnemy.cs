using System.Collections;
using System.Linq;
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
        _agent.speed = data.GetSpeed;
        _agent.stoppingDistance = data.GetAttackRange;
        FocusPlayer = data.GetFocusPlayer;
        Healthpoint = data.GetHealth;
        
        stateRenderer = m_stateSystem.GetComponent<Renderer>();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        state = FocusPlayer ? State.Chase : State.Neutral;

        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        _agent.SetDestination(Player.transform.position);

        base.Update();

        StateManagement();
    }
    
    private void Reset()
    {
        Healthpoint = data.GetHealth;
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = data.GetSpeed;
        _agent.stoppingDistance = data.GetAttackRange;
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
    
    // ReSharper disable Unity.PerformanceAnalysis
    private void StateManagement()
    {
        switch (state)
        {
            case State.Neutral:
                break;
            case State.Chase:
                _agent.SetDestination(Player.transform.position);
                break;
            case State.Attack:
                Attack(Shot, data.GetAttackSpeed);
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
        
        if (Vector3.Distance(transform.position, Player.transform.position) <= data.GetRangeDetection && 
            Vector3.Distance(transform.position, Player.transform.position) > data.GetAttackRange)
        {
            state = State.Chase;
        }
        else
        {
            state = State.Attack;
        }
    }

    protected override void Chase()
    {
        if (state == State.Dying)
            return;

        _agent.SetDestination(Player.transform.position);
        
        // stateRenderer.material.color = Color.yellow;
        // m_stateSystem.gameObject.SetActive(true);
    }

    private void Shot()
    {
        GameObject shot = Instantiate(m_bullet, m_gun.transform.position, Quaternion.identity);
        shot.GetComponent<EnemyBulletController>().SetDirection(Player.transform);
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
        
        if (Healthpoint <= 0)
        {
            state = State.Dying;
        }
    }
}