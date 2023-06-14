using UnityEngine;
using UnityEngine.AI;

namespace Enemy.Slime
{
    public class CheckingSpawn : MonoBehaviour
    {
        private NavMeshAgent agent;
        private NavMeshPath navMeshPath;
        private Transform player;
        
        // Start is called before the first frame update
        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            navMeshPath = new NavMeshPath();
            player = GameObject.FindGameObjectWithTag("Player").transform;
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
            agent.SetDestination(player.position);

            NavMeshPath path = new NavMeshPath();
            
            agent.CalculatePath(player.position, path);
            
            if (!agent.CalculatePath(player.transform.position, navMeshPath))
            {
                return false;
            }

            switch (navMeshPath.status)
            {
                case NavMeshPathStatus.PathPartial:
                case NavMeshPathStatus.PathInvalid:
                    return false;
                    break;
            }

            return true;
        }
    }
}