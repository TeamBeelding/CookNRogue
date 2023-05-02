using UnityEngine;
using UnityEngine.AI;

namespace Enemy.Slime
{
    public class SlimeCheckingMinimoyzSpawn : MonoBehaviour
    {
        private NavMeshAgent _agent;
        
        // Start is called before the first frame update
        private void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
        }
        
        public void SetTransformPosition(Vector3 position)
        {
            transform.position = position;
        }

        public bool IsPathValid(Vector3 destination)
        {
            _agent.SetDestination(destination);

            return _agent.pathStatus == NavMeshPathStatus.PathComplete;
        }
    }
}
