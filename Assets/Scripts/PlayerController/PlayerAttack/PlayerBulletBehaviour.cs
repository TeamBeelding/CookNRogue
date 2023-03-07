using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletBehaviour : MonoBehaviour
{
    public float heavyDamage;
    public float lightDamage;
    public float speed;
    public float drag = 1;
    protected bool HasHit = false;
    protected GameObject hitObject;
    public PlayerAttack playerAttack;
    Sprite sprite;
    //public Vector3 gravity;
    public Vector3 direction;
    public bool destroyOnHit = true;

    protected virtual void Start()
    {
        /*ParticleSystem.MainModule part = GetComponentInChildren<ParticleSystem>().main;
        part.startColor = color;

        rb.drag = drag;
        rb.velocity = direction * speed;*/
        Destroy(gameObject, 1f);
    }

    protected virtual void FixedUpdate()
    {
        //rb.AddForce(gravity * rb.mass);
        
        transform.Translate(direction*speed*0.1f);
        speed *= drag;
    }

    public void ResetStats()
    {
        //rb.drag = 0;
        //HeavyDamage = 1;
        gameObject.transform.localScale = new Vector3(1,1,1);
    }

    protected virtual void OnDestroy()
    {
        if(HasHit)
        {
            playerAttack.ApplyOnHitEffects(transform.position, hitObject, direction);
        }
        else
        {
            playerAttack.ApplyOnHitEffects(transform.position);
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            HasHit= true;
            hitObject = other.gameObject;
            if (other.GetComponent<Enemy>())
                other.GetComponent<Enemy>().TakeDamage(heavyDamage);

            if(destroyOnHit)
            {
                Debug.Log("destroy");
                Destroy(gameObject);
            }


        }
    }

    
}
