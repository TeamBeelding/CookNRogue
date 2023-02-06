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
    
    [SerializeField] 
    private Transform[] paths;
    
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
        
        // TODO : demander au prof une autre solution
        _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
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

    #region State
    private void StateManagement()
    {
        // Change the state of the enemy and call the corresponding function or colorize the enemy
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
        // If the enemy is not focused on the player, it will detect the player if he is in the detection range
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
            // Switch between chase and attack state depending on the distance between the enemy and the player 
            state = Vector3.Distance(transform.position, player.transform.position) <= data.GetAttackRange() ? State.Attack : State.Chase;
        }
    }
    
    #endregion
    
    #region NeutralState
    
    private void Pathing()
    {
        if (paths.Length == 0)
            return;
    
        _agent.SetPath(paths[0].GetComponent<NavMeshPath>());
    }
    private void GoToNextPoint()
    {
        
    }
    
    #endregion

    #region AttackState
    
    // ReSharper disable Unity.PerformanceAnalysis
    public void Attack()
    {
        if (_canAttack)
        {
            GameObject shot = Instantiate(bullet, gun.transform.position, Quaternion.identity);
            shot.GetComponent<EnemyBulletController>().SetDirection(player.transform);
            _canAttack = false;
            StartCoroutine(AttackTimer());
        }
    }
    
    #endregion
    
    #region TakeDamage

    public void TakeDamage(float damage = 1)
    {
        if (state == State.Neutral)
            state = State.Chase;

        ReduiceHealth(damage);
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
        _rigidbody.constraints = RigidbodyConstraints.None;
        
        StopCoroutine(StoppingForce());
        
        Vector3 direction = transform.position - player.transform.position;
        direction.Normalize();
        
        // transform.Translate(direction * data.GetRecoilForce() * Time.deltaTime);
        
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
    
    // Color the enemy red for a short time to indicate that he has been hit
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
    
    #endregion

    // Stop the enemy from moving after receiving a recoil force
    private IEnumerator StoppingForce()
    {
        yield return new WaitForSeconds(.5f);
        _rigidbody.velocity = Vector3.zero;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, data.GetRangeDetection());
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, data.GetAttackRange());
    }
}
