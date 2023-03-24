using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletBehaviour : MonoBehaviour
{
    public float _heavyDamage;
    public float _lightDamage;
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
    public int _ricochetNbr = 0;
    private float _initialSpeed;


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
        Debug.Log("Destroy");
        Destroy(gameObject);
    }
    public void ResetSpeed()
    {
      _speed = _initialSpeed;
    }
    protected virtual void OnTriggerEnter(Collider other)
    {

        
            

        

        if (other.GetComponent<EnemyController>())
        {
            _HasHit = true;
            _hitObject = other.gameObject;
            ApplyCorrectOnHitEffects();
             other.GetComponent<EnemyController>().TakeDamage(_heavyDamage);

            if (_ricochetNbr > 0)
            {
                _ricochetNbr--;
                ResetSpeed();
                Debug.Log(_ricochetNbr);
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

    
}
