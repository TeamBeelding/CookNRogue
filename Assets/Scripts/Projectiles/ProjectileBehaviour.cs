using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ProjectileBehaviour : MonoBehaviour
{

    public float speed;
    public float drag;
    Rigidbody rb;
    Sprite sprite;
    public Vector3 gravity;
   
   
    private void Start()
    {
        //ParticleSystem.MainModule part = GetComponentInChildren<ParticleSystem>().main;
        //part.startColor = color;
        rb = GetComponent<Rigidbody>();
        rb.drag = drag;
        rb.velocity =  new Vector3(speed, 0, 0);
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
}
