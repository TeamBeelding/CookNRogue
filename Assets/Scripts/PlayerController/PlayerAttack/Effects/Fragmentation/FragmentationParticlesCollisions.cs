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
    [Range(0f, 100f)]
    [SerializeField] private float _damage_percentage;
    [SerializeField] private LayerMask _mask;
    public GameObject DamageUI;
    void Start()
    {
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
                Debug.Log((_damage_percentage / 100));
                float totalDamage = PlayerRuntimeData.GetInstance().data.AttackData.AttackDamage * (_damage_percentage / 100);
                totalDamage = (int)totalDamage;
                enemyController.TakeDamage(totalDamage);
                GameObject UIDAMAGE = Instantiate(DamageUI, other.transform.position + (Vector3.up * 3) + GetCameraDirection() * 0.5f, Quaternion.identity);
                UIDAMAGE.GetComponentInChildren<TextMeshProUGUI>().text = totalDamage.ToString();
            }
            //kaboom(other.transform.position);
            i++;
        }

        foreach(ParticleCollisionEvent PartEvent in _collisionEvents)
        {
            Vector3 pos = PartEvent.intersection;
            SetUpDecal(pos);
        }
        
    }

    private void SetUpDecal(Vector3 pos)
    {
        if (!DecalManager.instance || PlayerRuntimeData.GetInstance().data.AttackData.AttackColor == null)
            return;

        BulletDecal decal = DecalManager.instance.GetAvailableDecal();
        decal.Init(Color.yellow);
        decal.transform.position = pos;
        decal.transform.rotation = Quaternion.Euler(90, 0, 0);
    }

    Vector3 GetCameraDirection()
    {
        Vector3 dir = Camera.main.transform.position - transform.position;
        return dir;
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
