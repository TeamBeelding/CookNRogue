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
            print(IsPathValid() ? "<color=green>Path available</color>" : "<color=red>Path not available</color>");
        }
        
        public void SetTransformPosition(Vector3 position)
        {
            spawnPosition.transform.position = position;
        }

        public bool IsPathValid()
        {
            spawnPosition.CalculatePath(player.position, navMeshPath);
        
            return navMeshPath.status == NavMeshPathStatus.PathComplete;
        }
    }
}