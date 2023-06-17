using System;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy.Slime
{
    public class CheckingSpawn : MonoBehaviour
    {
        // private NavMeshAgent agent;
        // private NavMeshPath navMeshPath;
        private Transform player;
        
        // Start is called before the first frame update
        private void Start()
        {
            // agent = GetComponent<NavMeshAgent>();
            // navMeshPath = new NavMeshPath();
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        public void SetTransformPosition(Vector3 position)
        {
            // agent.ResetPath();
            //
            // agent.enabled = false;
            // agent.transform.position = position;
            // agent.enabled = true;
            
            transform.position = position;
        }

        public bool IsPathValid()
        {
            // agent.SetDestination(player.position);

            // agent.CalculatePath(player.position, navMeshPath);
            //
            // if (navMeshPath.status == NavMeshPathStatus.PathComplete)
            //     return true;

            return false;
        }
        
        public bool CanThrowHere()
        {
            RaycastHit hit;
            
            if (Physics.Raycast(transform.position, player.position - transform.position, out hit, Vector3.Distance(transform.position, player.position)))
            {
                if (hit.collider.CompareTag("Player"))
                    return true;
            }

            return false;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, player.position - transform.position);
        }
    }
}