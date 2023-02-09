using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ProjectileBehaviour : MonoBehaviour
{
    public float heavyDamage;
    public float lightDamage;
    public float speed;
    public float drag;
    private bool HasHit = false;
    private GameObject hitObject;
    public PlayerAttack playerAttack;
    Rigidbody rb;
    Sprite sprite;
    public Vector3 gravity;
    public Vector3 direction;


    private void Start()
    {
        //ParticleSystem.MainModule part = GetComponentInChildren<ParticleSystem>().main;
        //part.startColor = color;
        rb = GetComponent<Rigidbody>();
        rb.drag = drag;
        rb.velocity = direction * speed;
        Destroy(gameObject, 5f);
    }

    void FixedUpdate()
    {
        rb.AddForce(gravity * rb.mass);
    }

    void ResetStats()
    {
        rb.drag = 0;
        gameObject.transform.localScale = new Vector3(1,1,1);
    }

    private void OnDestroy()
    {
        if(HasHit)
        {
            playerAttack.ApplyOnHitEffects(hitObject.transform.position, this.gameObject);
        }
        else
        {
            playerAttack.ApplyOnHitEffects(transform.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            HasHit= true;
            hitObject = other.gameObject;
            Destroy(gameObject);
            other.GetComponent<Enemy>().TakeDamage(heavyDamage);
        }
    }

    
}
