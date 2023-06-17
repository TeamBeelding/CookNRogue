using System;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy.Slime
{
    public class CheckingSpawn : MonoBehaviour
    {
        private NavMeshAgent agent;
        private NavMeshPath navMeshPath;
        private Transform player;
        [SerializeField] private Material validMaterial;
        
        // Start is called before the first frame update
        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            navMeshPath = new NavMeshPath();
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        private void Update()
        {
            if (IsPathValid())
                GetComponentInChildren<Renderer>().material.color = Color.blue;
            else
                GetComponentInChildren<Renderer>().material.color = Color.red;
        }

        public void SetTransformPosition(Vector3 position)
        {
            agent.ResetPath();
            
            agent.enabled = false;
            agent.transform.position = position;
            agent.enabled = true;
        }

        public bool IsPathValid()
        {
            // agent.SetDestination(player.position);

            agent.CalculatePath(player.position, navMeshPath);

            if (navMeshPath.status == NavMeshPathStatus.PathComplete)
                return true;

            return false;
        }
    }
}