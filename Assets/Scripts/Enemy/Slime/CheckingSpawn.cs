using UnityEngine;

namespace Enemy.Slime
{
    public class CheckingSpawn : MonoBehaviour
    {
        private Transform player;
        
        // Start is called before the first frame update
        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        public void SetTransformPosition(Vector3 position)
        {
            transform.position = position;
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

        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (player == null)
                return;
            
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, player.position - transform.position);
        }
        #endif
    }
}