using UnityEngine;
using UnityEngine.AI;

namespace Enemy.Slime
{
    public class CheckingSpawn : MonoBehaviour
    {
        private NavMeshAgent spawnPosition;
        private NavMeshPath navMeshPath;
        private Transform player;
        
        // Start is called before the first frame update
        private void Start()
        {
            spawnPosition = GetComponent<NavMeshAgent>();
            navMeshPath = new NavMeshPath();
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        public void Update()
        {
            
        }
        
        public void SetTransformPosition(Vector3 position)
        {
            spawnPosition.enabled = false;
            spawnPosition.transform.position = position;
            spawnPosition.enabled = true;
        }

        public bool IsPathValid()
        {
            spawnPosition.CalculatePath(player.position, navMeshPath);
        
            return navMeshPath.status == NavMeshPathStatus.PathComplete;
        }
    }
}