using Sirenix.Utilities;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

public class PlayerBulletBehaviour : MonoBehaviour
{
    public float _damage;
    private float _initialDamage;
    public bool _isCritical;
    public float _speed = 4;
    public float _drag = 1;
    protected bool _HasHit = false;
    protected GameObject _hitObject;
    public PlayerAttack _playerAttack;
    Sprite sprite;
    //public Vector3 gravity;
    public Vector3 _direction;
    public bool destroyOnHit = true;
    public int bouncingNbr = 0;

    public int _ricochetNbr ;
    private float _initialSpeed;

    public float _maxDistance;
    public LayerMask _sphereMask;
    public LayerMask _rayMask;
    public GameObject Explosion;
    public List<GameObject> VFX = new List<GameObject>();
    public bool HasExploded = false;
    public virtual void Init()
    {
        _initialSpeed = _speed;
        _initialDamage = _damage;
        HasExploded = false;
    }

    protected virtual void FixedUpdate()
    {
        //rb.AddForce(gravity * rb.mass);
        
        transform.Translate(_direction * _speed*0.1f);
        _speed *= _drag;
    }

    public virtual void ResetStats()
    {
        //rb.drag = 0;
        //HeavyDamage = 1;
        gameObject.transform.localScale = new Vector3(1,1,1);
        _speed = _initialSpeed;
        _damage = _initialDamage;
        _HasHit = false;
        destroyOnHit = true;
        bouncingNbr = 0;
        _ricochetNbr = 0;
    }
    public virtual void ExplosionEffect()
    {
        if (HasExploded)
            return;

        Splash explosion = SplashManager.instance.GetAvailableSplash();
        explosion.transform.position = transform.position;
        explosion.TriggerSplash();

        HasExploded = true;
        if (!_HasHit)
        {
            ApplyCorrectOnHitEffects();
        }
    }
    protected void DisableBullet()
    {
        if(!HasExploded)
            ExplosionEffect();

        BoomerangBehaviour boomerang = null;
        TryGetComponent(out boomerang);
        if(boomerang != null)
            Destroy(boomerang);

        foreach(GameObject vfx in VFX)
        {
            Destroy(vfx);
        }

        gameObject.SetActive(false);
    }
    public void ResetSpeed()
    {
      _speed = _initialSpeed;
    }
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<EnemyController>())
        {
            _HasHit = true;
            _hitObject = other.gameObject;
            ApplyCorrectOnHitEffects();

            SetUpDecal(other.transform.position);

            _damage = (int)_damage;
             other.GetComponentInParent<EnemyController>().TakeDamage(_damage, _isCritical);

            if (_ricochetNbr > 0)
            {
                _ricochetNbr--;
                BulletRicochet(transform.position, _hitObject, _direction);
                ResetSpeed();
                return;
            }

            if (destroyOnHit)
            {
                //Destroy(gameObject);
                DisableBullet();
            }


        }
        else
        {
            _HasHit = false;
            ApplyCorrectOnHitEffects();

            if (bouncingNbr > 0)
            {
                bouncingNbr--;
                RaycastHit hit;
                if (Physics.Raycast(transform.position - _direction, _direction, out hit,3,9))
                {
                    Debug.Log(hit.transform.name);
                    _direction = Vector3.Reflect(_direction, hit.normal);
                }
                
                bouncingNbr--;
            }
            else
            {
                //Destroy(gameObject);
                DisableBullet();
            }
        }
    }

    void BulletRicochet(Vector3 Position, GameObject HitObject, Vector3 direction)
    {
        if (!HitObject)
            return;

        Collider[] hitColliders = Physics.OverlapSphere(Position, _maxDistance, _sphereMask);

        float closest = 999f;
        float distance = 0f;
        GameObject closestEnemy = HitObject;

        if (HitObject.GetComponent<EnemyController>())
            HitObject.layer = 2;

        
        foreach (Collider hitCollider in hitColliders)
        {
            
            if (hitCollider.gameObject != HitObject && hitCollider.CompareTag("Enemy"))
            {
                
                distance = Vector3.Distance(hitCollider.gameObject.transform.position, Position);

                Vector3 rayDirection = (hitCollider.gameObject.transform.position - Position).normalized;
                
                if (distance < closest)
                {
                    
                    closest = distance;
                    closestEnemy = hitCollider.gameObject;
                    Debug.Log(closestEnemy);
                }
            }
            

        }
        HitObject.layer = 0;
        if (closestEnemy != HitObject)
        {
            CancelInvoke("DisableBullet");
            Invoke("DisableBullet", 1);
            _direction = (closestEnemy.gameObject.transform.position - HitObject.transform.position).normalized;
            destroyOnHit = true;

            //VFX
            foreach (IIngredientEffects effect in PlayerRuntimeData.GetInstance().data.AttackData.AttackEffects)
            {
                if (effect is Ricochet ricochet)
                {
                    GameObject ricochetPart = Instantiate(ricochet.RicochetParticles, Position, Quaternion.identity);

                    Destroy(ricochetPart, 0.5f);
                    Debug.Log("ricochet");
                }
            }
        }
        else
        {
            DisableBullet();
        }
        
        
    }


    void ApplyCorrectOnHitEffects()
    {
        SetUpDecal(transform.position);

        if (_HasHit)
        {
            _playerAttack.ApplyOnHitEffects(transform.position, _hitObject, _direction);
        }
        else
        {
            _playerAttack.ApplyOnHitEffects(transform.position);
        }
    }

    private void SetUpDecal(Vector3 pos)
    {
        if (!DecalManager.instance)
            return;

        BulletDecal decal = DecalManager.instance.GetAvailableDecal();
        decal.Init((Color)PlayerRuntimeData.GetInstance().data.AttackData.AttackColor);
        decal.transform.position = pos;
        decal.transform.rotation = Quaternion.Euler(90, 0, 0);
    }

    private void Reset()
    {
        _speed= 1;
        _drag = 1;
    }

}
