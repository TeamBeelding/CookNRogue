using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public enum State
    {
        Chase,
        Neutral,
        Attack
    }

    public State state;
    
    [SerializeField]
    private GameObject player;
    [SerializeField] 
    private EnemyData data;

    private Renderer _rend;
    private Rigidbody _rigidbody;
    private MeshRenderer _meshRenderer;
    private NavMeshAgent _agent;
    
    [SerializeField]
    private GameObject gun;
    [SerializeField]
    private GameObject bullet;
    
    private bool _focusPlayer = false;
    private bool _canAttack = true;
    private float healthpoint;

    private void Awake()
    {
        _rend = GetComponent<Renderer>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _rigidbody = GetComponent<Rigidbody>();
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = data.GetSpeed();
        _agent.stoppingDistance = data.GetAttackRange();
        _focusPlayer = data.GetFocusPlayer();

        healthpoint = data.GetHealth();
    }

    // Start is called before the first frame update
    protected void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        _rend.material.color = Color.white;

        if (_focusPlayer)
            state = State.Chase;
    }

    // Update is called once per frame
    protected void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            TakeDamage();
        
        StateManagement();
    }

    private void FixedUpdate()
    {
        AreaDetection();
    }

    public State GetState()
    {
        return state;
    }

    private void StateManagement()
    {
        switch (state)
        {
            case State.Chase:
                _agent.SetDestination(player.transform.position);
                _rend.material.color = Color.yellow;
                break;
            case State.Attack:
                _rend.material.color = Color.red;
                Attack();
                break;
            case State.Neutral:
                _rend.material.color = Color.green;
                break;
            default:
                _rend.material.color = Color.white;
                break;
        }
    }

    private void AreaDetection()
    {
        if (!_focusPlayer)
        {
            Collider[] col = Physics.OverlapSphere(transform.position, data.GetRangeDetection(), 
                LayerMask.NameToLayer("Player"));

            foreach (Collider c in col)
            {
                if (c.gameObject.CompareTag("Player"))
                {
                    _focusPlayer = true;
                    state = State.Chase;
                }
            }
        }
        else
        {
            state = Vector3.Distance(transform.position, player.transform.position) <= data.GetAttackRange() ? State.Attack : State.Chase;
        }
    }

    public void Attack()
    {
        if (_canAttack)
        {
            GameObject shot = Instantiate(bullet, gun.transform.position, Quaternion.identity);
            shot.GetComponent<BulletController>().SetDirection(player.transform);
            _canAttack = false;
            StartCoroutine(AttackTimer());
        }
    }

    public void TakeDamage()
    {
        if (state == State.Neutral)
            state = State.Chase;
        
        ReduiceHealth(1);
        KnockBack();
        StartCoroutine(ColorationFeedback());
    }

    private void ReduiceHealth(float damage)
    {
        damage = Mathf.Abs(damage);
        healthpoint -= damage;

        if (healthpoint <= 0)
        {
            KillEnemy();
        }
    }

    public void KnockBack()
    {
        StopCoroutine(StoppingForce());
        
        Vector3 direction = transform.position - player.transform.position;
        direction.Normalize();
        
        _rigidbody.AddForce(direction * data.GetRecoilForce(), ForceMode.Impulse);
        StartCoroutine(StoppingForce());
    }

    public void ColorFeedback()
    {
        StartCoroutine(ColorationFeedback());
    }

    public void KillEnemy()
    {
        Destroy(gameObject);
    }

    private IEnumerator ColorationFeedback()
    {
        for (int i = 0; i < 5; i++)
        {
            _meshRenderer.enabled = false;
            yield return new WaitForSeconds(0.2f);
            _meshRenderer.enabled = true;
            yield return new WaitForSeconds(0.2f);
        }
    }

    private IEnumerator AttackTimer()
    {
        yield return new WaitForSeconds(data.GetAttackSpeed());
        _canAttack = true;
    }

    private IEnumerator StoppingForce()
    {
        yield return new WaitForSeconds(.5f);
        _rigidbody.Sleep();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, data.GetRangeDetection());
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, data.GetAttackRange());
    }
}