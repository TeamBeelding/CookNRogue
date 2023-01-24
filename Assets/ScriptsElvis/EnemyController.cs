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
    
    // Start is called before the first frame update
    protected void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        agent.speed = data.speed;
        agent.stoppingDistance = data.range;
    }

    // Update is called once per frame
    protected void Update()
    {
        agent.SetDestination(player.transform.position);
    }
}
