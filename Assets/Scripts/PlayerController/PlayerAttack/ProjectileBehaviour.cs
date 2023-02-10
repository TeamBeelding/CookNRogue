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
    [SerializeField]
    Rigidbody rb;
    Sprite sprite;
    public Vector3 gravity;
    public Vector3 direction;


    private void Start()
    {
        //ParticleSystem.MainModule part = GetComponentInChildren<ParticleSystem>().main;
        //part.startColor = color;
       
        rb.drag = drag;
        rb.velocity = direction * speed;
        Destroy(gameObject, 5f);
    }

    void FixedUpdate()
    {
        rb.AddForce(gravity * rb.mass);
    }

    public void ResetStats()
    {
        rb.drag = 0;
        heavyDamage = 1;
        gameObject.transform.localScale = new Vector3(1,1,1);
    }

    private void OnDestroy()
    {
        if(HasHit)
        {
            playerAttack.ApplyOnHitEffects(transform.position, hitObject, direction);
        }
        else
        {
            //playerAttack.ApplyOnHitEffects(transform.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            HasHit= true;
            hitObject = other.gameObject;

            if (other.GetComponent<Enemy>())
                other.GetComponent<Enemy>().TakeDamage(heavyDamage);


            Destroy(gameObject);

        }
    }

    
}
