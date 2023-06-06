using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(ParticleSystem))]
public class FragmentationParticlesCollisions : MonoBehaviour
{
    ParticleSystem _part;
    List<ParticleCollisionEvent> _collisionEvents;
    [SerializeField] private float m_ExplosionForce;
    [SerializeField] private float m_ExplosionRadius;
    [SerializeField] private float damage;
    [SerializeField] private LayerMask _mask;

    void Start()
    {
        _part = GetComponent<ParticleSystem>();
        _collisionEvents = new List<ParticleCollisionEvent>();
        Destroy(gameObject,_part.main.duration);
    }
    void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = _part.GetCollisionEvents(other, _collisionEvents);

        Rigidbody rb = other.GetComponent<Rigidbody>();
        int i = 0;

        while (i < numCollisionEvents)
        {
           EnemyController enemyController = other.GetComponent<EnemyController>();

            if (enemyController != null)
            {
                enemyController.TakeDamage(damage);
            }
            kaboom(other.transform.position);
            i++;
        }
        
    }

    

    void kaboom(Vector3 Position)
    {

        
        Collider[] hitColliders = Physics.OverlapSphere(Position, m_ExplosionRadius,_mask);

        foreach (Collider hitCollider in hitColliders)
        {
            //float distance = (Position - hitCollider.transform.position).magnitude;


            Rigidbody rb = hitCollider.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddExplosionForce(m_ExplosionForce, Position, m_ExplosionRadius);
                
            }



        }


    }

}
