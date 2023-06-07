using System.Collections.Generic;
using UnityEngine;
using TMPro;


[RequireComponent(typeof(ParticleSystem))]
public class FragmentationParticlesCollisions : MonoBehaviour
{
    ParticleSystem _part;
    List<ParticleCollisionEvent> _collisionEvents;
    [SerializeField] private float m_ExplosionForce;
    [SerializeField] private float m_ExplosionRadius;
    private float _damage = 0f;
    [SerializeField] private LayerMask _mask;
    PlayerAttack _playerAttack;
    public GameObject DamageUI;
    void Start()
    {
        _playerAttack = FindObjectOfType<PlayerAttack>();
        float damage = _playerAttack._damage;
        SetDamage(damage);

        _part = GetComponent<ParticleSystem>();
        _collisionEvents = new List<ParticleCollisionEvent>();
        Destroy(gameObject,_part.main.duration);
    }
    void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = _part.GetCollisionEvents(other, _collisionEvents);

        int i = 0;

        while (i < numCollisionEvents)
        {
            EnemyController enemyController = other.GetComponentInParent<EnemyController>();
            
            if (enemyController != null)
            {
               Debug.Log("enemy");
                enemyController.TakeDamage(_damage);
                GameObject UIDAMAGE = Instantiate(DamageUI, other.transform.position + (Vector3.up * 3) + GetCameraDirection() * 0.5f, Quaternion.identity);
                UIDAMAGE.GetComponentInChildren<TextMeshProUGUI>().text = _damage.ToString();
            }
            kaboom(other.transform.position);
            i++;
        }
        
    }

    Vector3 GetCameraDirection()
    {
        Vector3 dir = Camera.main.transform.position - transform.position;
        return dir;
    }

    public void SetDamage(float damage)
    {
        _damage = damage;
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
