using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public sealed class MissileBehaviour : MonoBehaviour
{
    [SerializeField] float _startHeight;
    [SerializeField] float _fallSpeed;
    [SerializeField] private float _radiusExplosion;

    bool _hasExploded = false;
    [SerializeField] GameObject GFX;

    [SerializeField] Transform _vfxContainer;

    [SerializeField,ColorUsage(true, true)]
    Color _explosionColor;

    ParticleSystem[] _particles;

    [SerializeField] ParticleSystem _fireParticles1;
    [SerializeField] ParticleSystem _fireParticles2;
    [SerializeField] ParticleSystem _markParticles;

    GameObject _decal;
    MissileBoss _missileBoss;

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
    }
    
    public void Init(GameObject decal, MissileBoss missileBoss)
    {
        _decal = decal;
        _missileBoss = missileBoss;

        transform.position = new Vector3(transform.position.x,_startHeight,transform.position.z);
        _hasExploded = false;
        GFX.SetActive(true);
        StartCoroutine(MissileFall());
    }

    IEnumerator MissileFall()
    {
        while (!_hasExploded)
        {
            transform.position += Vector3.down * _fallSpeed * 0.1f;
            yield return new WaitForFixedUpdate();
        }

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
