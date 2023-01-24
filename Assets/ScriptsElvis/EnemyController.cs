using System;
using System.Collections;
using System.Collections.Generic;
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
    private NavMeshAgent _agent;
    private bool _focusPlayer = false;

    private void Awake()
    {
        _rend = GetComponent<Renderer>();
        
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = data.GetSpeed();
        _agent.stoppingDistance = data.GetAttackRange();
        _focusPlayer = data.GetFocusPlayer();
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
                break;
            case State.Attack:
                Attack();
                break;
            default:
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

    protected void Attack()
    {
        Debug.Log("Attack");
    }

    private void TakeDamage()
    {
        StartCoroutine(ColorationFeedback());
    }

    private IEnumerator ColorationFeedback()
    {
        for (int i = 0; i < 5; i++)
        {
            _rend.material.color = Color.red;
            yield return new WaitForSeconds(0.2f);
            _rend.material.color = Color.white;
            yield return new WaitForSeconds(0.2f);

        }
    }
}
