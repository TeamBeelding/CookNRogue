using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using static UnityEngine.UI.Image;

public class Cauldron_Explosion : MonoBehaviour
{
    [SerializeField] float _explosionTimer;
    [SerializeField] float _explosionRadius;

    [Space(20)]
    [Header("DAMAGE")]
    [SerializeField] int _maxEnnemyDamage;
    [SerializeField] int _maxPlayerDamage;
    [SerializeField] AnimationCurve _damageByDistanceCurve;

    [Space(20)]
    [Header("VFX")]
    [SerializeField] Transform _vfxContainer;
    ParticleSystem[] _particles;
    [SerializeField] ParticleSystem _fireParticles1;
    [SerializeField] ParticleSystem _fireParticles2;
    [SerializeField] DecalProjector _explosionDecal;
    

    bool _canExplode = true;

    [ColorUsage(true,true)]
    Color _explosionColor;

    [SerializeField, ColorUsage(true, true)]
    Color _cauldronDefaultColor;
    [SerializeField] bool _useCauldronColor;

    [SerializeField, ColorUsage(true, true)] Color _explosionDecalColor;

    

    private void Start()
    {
        _particles = _vfxContainer.GetComponentsInChildren<ParticleSystem>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.parent.GetComponent<PlayerBulletBehaviour>())
        {
            if (!_canExplode)
                return;

            _explosionColor = GetDesiredColor();
            _explosionColor.a = 1;

            var renderer = _fireParticles1.GetComponent<ParticleSystemRenderer>();
            renderer.material.SetColor("_ExplosionColor",_explosionColor);
            renderer = _fireParticles2.GetComponent<ParticleSystemRenderer>();
            renderer.material.SetColor("_ExplosionColor", _explosionColor);

          
            StartCoroutine(Explosion(_explosionTimer));
        }
    }

    void InitDecal()
    {
        _explosionDecal.gameObject.SetActive(true);
        _explosionDecal.material = new Material(_explosionDecal.material);
        var mat = _explosionDecal.material;
        mat.SetColor("_Color", _explosionDecalColor);
    }

    Color GetDesiredColor()
    {
        if(_useCauldronColor)
            return _cauldronDefaultColor;
        else return (Color)PlayerRuntimeData.GetInstance().data.AttackData.AttackColor;
    }

    IEnumerator Explosion(float timer)
    {
        _canExplode = false;
        yield return new WaitForSecondsRealtime(timer);

        TriggerParticles();

        RaycastHit[] hits = Physics.SphereCastAll(transform.position, _explosionRadius, Vector3.up);

        foreach(RaycastHit hit in hits)
        {
            if (hit.transform.GetComponent<PlayerHealth>())
            {
                int damage = (int)CalculatePlayerDamage(hit.transform.position);
                hit.transform.GetComponent<PlayerHealth>().TakeDamage(damage);
                continue;
            }

            if (!hit.transform.parent)
                continue;

            if (hit.transform.parent.GetComponent<EnemyController>())
            {
                int damage = (int)CalculateEnemyDamage(hit.transform.position);
                hit.transform.GetComponent<EnemyController>().TakeDamage(damage);
                continue;
            }
        }
        yield return new WaitForSeconds(0.5f);
        InitDecal();
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    public float CalculateEnemyDamage(Vector3 position)
    {
        float distance = Vector3.Distance(transform.position, position);

        float damage = _damageByDistanceCurve.Evaluate(distance / _explosionRadius) * _maxPlayerDamage;

        return damage;
    }
    public float CalculatePlayerDamage(Vector3 position)
    {
        float distance = Vector3.Distance(transform.position, position);

        float damage = _damageByDistanceCurve.Evaluate(distance / _explosionRadius) * _maxPlayerDamage;

        return damage;
    }

    void TriggerParticles()
    {
        foreach (var particle in _particles)
        {
            particle.Play();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _explosionRadius);
    }

}
