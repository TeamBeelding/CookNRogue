using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class Cauldron_Explosion : MonoBehaviour
{
    [SerializeField] [Range(0.1f,4f)] float _triggerRadius;
    [SerializeField] SphereCollider _sphereCollider;
    [SerializeField] float _explosionTimer;
    [SerializeField] float _explosionRadius;

    [Space(20)]
    [Header("DAMAGE")]
    [SerializeField] int _maxDamage;
    [SerializeField] AnimationCurve _damageCurve;

    float _damage_radius_increment;

    private void Start()
    {
        _sphereCollider.radius = _triggerRadius;
        _damage_radius_increment = _triggerRadius / 3;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerBulletBehaviour>())
        {
            StartCoroutine(Eplosion(_explosionTimer));
        }
    }

    IEnumerator Eplosion(float timer)
    {
        yield return new WaitForSecondsRealtime(timer);
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, _explosionRadius, Vector3.up);

        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.GetComponent<EnemyController>())
            {
                int damage = (int)CalculateDamage(hit.transform.position);
                hit.transform.GetComponent<EnemyController>().TakeDamage(damage);
            }
        }
        Debug.Log("kaboom");
    }

    public float CalculateDamage(Vector3 position)
    {
        float distance = Vector3.Distance(transform.position, position);

        float damage = _damageCurve.Evaluate(distance / _explosionRadius) * _maxDamage;

        return damage;
    }
    
}
