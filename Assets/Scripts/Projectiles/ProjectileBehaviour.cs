using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ProjectileBehaviour : MonoBehaviour
{
    [SerializeField]
    ProjectileData data;
    Rigidbody rb;
    private void Start()
    {
        
        ParticleSystem.MainModule part = GetComponentInChildren<ParticleSystem>().main;
        part.startColor = data.color;
        rb = GetComponent<Rigidbody>();
        rb.drag = data.drag;
        rb.mass = data.mass;
        rb.velocity = new Vector3(data.speed, 0, 0);
    }
    

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject,1f);
    }
}
