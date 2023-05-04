using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletBehaviour : MonoBehaviour
{
    public float _damage;
    public bool _isCritical;
    public float _speed;
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


    protected virtual void Start()
    {
        /*ParticleSystem.MainModule part = GetComponentInChildren<ParticleSystem>().main;
        part.startColor = color;

        rb.drag = drag;
        rb.velocity = direction * speed;*/
        Invoke("DestroyBullet", 1);
        _initialSpeed = _speed;
    }

    protected virtual void FixedUpdate()
    {
        //rb.AddForce(gravity * rb.mass);
        
        transform.Translate(_direction * _speed*0.1f);
        _speed *= _drag;
    }

    public void ResetStats()
    {
        //rb.drag = 0;
        //HeavyDamage = 1;
        gameObject.transform.localScale = new Vector3(1,1,1);
    }

    protected virtual void OnDestroy()
    {
        if (!_HasHit)
        {
            ApplyCorrectOnHitEffects();
        }
    }
    public void DestroyBullet()
    {
        //Debug.Log("Destroy");
        Destroy(gameObject);
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

             other.GetComponentInParent<EnemyController>().TakeDamage(_damage, _isCritical);

            if (_ricochetNbr > 0)
            {
                Debug.Log("ricochet");
                _ricochetNbr--;
                BulletRicochet(transform.position, _hitObject, _direction);
                ResetSpeed();
                return;
            }

            if (destroyOnHit)
            {
                Destroy(gameObject);
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
                if (Physics.Raycast(transform.position, _direction, out hit))
                {
                    _direction = Vector3.Reflect(_direction.normalized, hit.normal);
                }
                
                bouncingNbr--;
            }
            else
            {
                Destroy(gameObject);
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
            if (hitCollider.gameObject != HitObject && hitCollider.GetComponent<EnemyController>())
            {
                hitCollider.transform.gameObject.layer = 2;
                distance = Vector3.Distance(hitCollider.gameObject.transform.position, Position);

                Vector3 rayDirection = (hitCollider.gameObject.transform.position - Position).normalized;
                Debug.DrawLine(Position, Position + (rayDirection * (distance * 0.8f)), Color.cyan, 999);

                RaycastHit hit;
                bool hashit = Physics.Raycast(Position, rayDirection, out hit, distance * 0.8f, _rayMask);
                if (distance < closest)
                {
                    
                    closest = distance;
                    closestEnemy = hitCollider.gameObject;
                    Debug.Log(closestEnemy);
                }
                hitCollider.transform.gameObject.layer = 0;
            }
            

        }
        HitObject.layer = 0;
        if (closestEnemy != HitObject)
        {
            CancelInvoke("DestroyBullet");
            Invoke("DestroyBullet", 1);
            _direction = (closestEnemy.gameObject.transform.position - HitObject.transform.position).normalized;
            destroyOnHit = true;
        }
        else
        {
            DestroyBullet();
        }

    }


    void ApplyCorrectOnHitEffects()
    {
        if (_HasHit)
        {
            _playerAttack.ApplyOnHitEffects(transform.position, _hitObject, _direction);
        }
        else
        {
            _playerAttack.ApplyOnHitEffects(transform.position);
        }
    }

    private void Reset()
    {
        _speed= 1;
        _drag = 1;
    }

}
