using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public sealed class MissileBehaviour : MonoBehaviour
{
    [SerializeField] float _maxHeight;
    [SerializeField] float _fallSpeed;

    [SerializeField] AnimationCurve _heightCurve;
    [SerializeField] AnimationCurve _forwardCurve;
    [SerializeField,Range(0.5f,3f)] float _missileSpeed;

    [SerializeField] private float _radiusExplosion;
    Collider _collider;
    bool _hasExploded = false;
    [SerializeField] GameObject GFX;

    [SerializeField] Transform _vfxContainer;

    [SerializeField,ColorUsage(true, true)]
    Color _explosionColor;

    ParticleSystem[] _particles;

    [SerializeField] ParticleSystem _fireParticles1;
    [SerializeField] ParticleSystem _fireParticles2;
    [SerializeField] ParticleSystem _markParticles;

    [SerializeField] ParticleSystem _smokeParticles;
    GameObject _decal;
    MissileBoss _missileBoss;
    Vector3 _target = Vector3.zero;

    private void OnEnable()
    {
        _particles = _vfxContainer.GetComponentsInChildren<ParticleSystem>();

        _explosionColor.a = 1;

        var renderer = _fireParticles1.GetComponent<ParticleSystemRenderer>();
        renderer.material.SetColor("_ExplosionColor", _explosionColor);
        renderer = _fireParticles2.GetComponent<ParticleSystemRenderer>();
        renderer.material.SetColor("_ExplosionColor", _explosionColor);
        renderer = _markParticles.GetComponent<ParticleSystemRenderer>();
        renderer.material.SetColor("_Color", _explosionColor);

        _collider = GetComponent<Collider>();
        
    }
    
    public void Init(GameObject decal, MissileBoss missileBoss, Vector3 target,Vector3 BossPosition)
    {
        _decal = decal;
        _missileBoss = missileBoss;
        _target = target;
        transform.position = BossPosition;
        _hasExploded = false;
        GFX.SetActive(true);
        _collider.enabled = false;
        _smokeParticles.Play();
        StartCoroutine(MissileFall());
    }

    IEnumerator MissileFall()
    {
        float progress = 0;
        Vector3 initialPos = transform.position;
        Vector3 dir = _target - initialPos;

        while (progress < 1 && !_hasExploded)
        {
            //OLD
            //transform.position += Vector3.down * _fallSpeed * 0.1f;

            //NEW
            Vector3 oldPosition = transform.position;
            transform.position = initialPos + dir.normalized * (dir.magnitude * _forwardCurve.Evaluate(progress)) + (Vector3.up * _heightCurve.Evaluate(progress) * _maxHeight);

            transform.LookAt(oldPosition);
            
            progress += Time.fixedDeltaTime * _missileSpeed;

            if(progress > 0.8f)
                _collider.enabled = true;

            if (progress > 1)
                progress = 1;

            yield return new WaitForFixedUpdate();
        }
        _smokeParticles.Stop();
        TriggerParticles();
        yield return new WaitForSeconds(0.5f);
        GFX.SetActive(false);
        yield return new WaitForSeconds(3f);
        _missileBoss.gameObject.SetActive(false);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        _hasExploded = true;

        if (Vector3.Distance(_missileBoss.transform.position, PlayerController.Instance.transform.position) <= _radiusExplosion)
        {
            PlayerController.Instance.TakeDamage(_missileBoss.damage);
        }

        _decal.SetActive(false);
    }

    void TriggerParticles()
    {
        foreach (var particle in _particles)
        {
            particle.Play();
        }
    }
}
