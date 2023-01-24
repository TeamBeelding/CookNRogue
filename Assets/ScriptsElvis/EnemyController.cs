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
    
    private NavMeshAgent agent;
    private bool focusPlayer = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = data.GetSpeed();
        agent.stoppingDistance = data.GetAttackRange();
        focusPlayer = data.GetFocusPlayer();
    }

    // Start is called before the first frame update
    protected void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (focusPlayer)
            state = State.Chase;
    }

    // Update is called once per frame
    protected void Update()
    {
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
                agent.SetDestination(player.transform.position);
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
        if (!focusPlayer)
        {
            Collider[] col = Physics.OverlapSphere(transform.position, data.GetRangeDetection(), 
                LayerMask.NameToLayer("Player"));

            foreach (Collider c in col)
            {
                if (c.gameObject.CompareTag("Player"))
                {
                    focusPlayer = true;
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
}
