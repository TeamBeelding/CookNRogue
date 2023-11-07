using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class Cauldron_Explosion : MonoBehaviour
{
    [SerializeField] float _explosionTimer;
    [SerializeField] float _explosionRadius;

    [Space(20)]
    [Header("DAMAGE")]
    [SerializeField] int _maxDamage;
    [SerializeField] AnimationCurve _damageCurve;
    float _damage_radius_increment;

    [Space(20)]
    [Header("VFX")]
    [SerializeField] Transform _vfxContainer;
    ParticleSystem[] _particles;
    [SerializeField] ParticleSystem _fireParticles;

    bool _canExplode = true;

    [SerializeField,ColorUsage(true,true)]
    Color _explosionColor;

    private void Start()
    {
        _damage_radius_increment = _explosionRadius / 3;
        _particles = _vfxContainer.GetComponentsInChildren<ParticleSystem>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.parent.GetComponent<PlayerBulletBehaviour>())
        {
            if (!_canExplode)
                return;

            _explosionColor = (Color)PlayerRuntimeData.GetInstance().data.AttackData.AttackColor;
            _explosionColor.a = 1;

            var renderer = _fireParticles.GetComponent<ParticleSystemRenderer>();
            renderer.material.color = _explosionColor;

            StartCoroutine(Explosion(_explosionTimer));
        }
    }

    IEnumerator Explosion(float timer)
    {
        _canExplode = false;
        yield return new WaitForSecondsRealtime(timer);

        TriggerParticles();

        RaycastHit[] hits = Physics.SphereCastAll(transform.position, _explosionRadius, Vector3.up);

        foreach(RaycastHit hit in hits)
        {
            if (!hit.transform.parent)
                continue;

            if (hit.transform.parent.GetComponent<EnemyController>())
            {
                int damage = (int)CalculateDamage(hit.transform.position);
                hit.transform.GetComponent<EnemyController>().TakeDamage(damage);
            }
        }
        yield return new WaitForSecondsRealtime(0.5f);
        Destroy(gameObject);
    }

    public float CalculateDamage(Vector3 position)
    {
        float distance = Vector3.Distance(transform.position, position);

        float damage = _damageCurve.Evaluate(distance / _explosionRadius) * _maxDamage;

        return damage;
    }

    void TriggerParticles()
    {
        foreach (var particle in _particles)
        {
            particle.Play();
        }
    }
    
}
