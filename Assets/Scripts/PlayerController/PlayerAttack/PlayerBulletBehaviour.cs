using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

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
    public GameObject Explosion;
    public GameObject DamageUI;

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
        GameObject explosion = Instantiate(Explosion, transform.position,Quaternion.identity);
        Destroy(explosion,1);
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

            _damage = (int)_damage;
             other.GetComponentInParent<EnemyController>().TakeDamage(_damage, _isCritical);

            GameObject UIDAMAGE = Instantiate(DamageUI, other.transform.position + (Vector3.up * 3) + GetCameraDirection() * 0.5f, Quaternion.identity);
            UIDAMAGE.GetComponentInChildren<TextMeshProUGUI>().text = _damage.ToString();
            //UIDAMAGE.GetComponentInChildren<TextMeshProUGUI>().text = "RATIO";

            Destroy(UIDAMAGE, 1);

            if (_ricochetNbr > 0)
            {
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
    Vector3 GetCameraDirection()
    {
        Vector3 dir = Camera.main.transform.position - transform.position;
        return dir;
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
            CancelInvoke("DestroyBullet");
            Invoke("DestroyBullet", 1);
            _direction = (closestEnemy.gameObject.transform.position - HitObject.transform.position).normalized;
            destroyOnHit = true;

            //VFX
            foreach (IIngredientEffects effect in PlayerRuntimeData.GetInstance().data.AttackData.AttackEffects)
            {
                if (effect is Ricochet ricochet)
                {
                    GameObject RicochetPart = Instantiate(ricochet.RicochetParticles, Position, Quaternion.identity);
                    Destroy(RicochetPart, 0.5f);
                    Debug.Log("ricochet");
                }
            }
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
