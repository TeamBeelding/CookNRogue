using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletController : MonoBehaviour
{
    [SerializeField]
    private EnemyBulletData _data;
    
    private bool isDirectionSet = false;
    private float speed;
    private Vector3 direction;
    
    private void Start()
    {
        speed = _data.GetSpeed();
    }
    
    // Update is called once per frame
    private void Update()
    {
        if (isDirectionSet)
            Move();
    }

    // Set direction of bullet
    public void SetDirection(Transform dir)
    {
        direction = dir.position - transform.position;
        direction.Normalize();
        isDirectionSet = true;
    }

    // Move bullet
    private void Move()
    {
        transform.position += direction * (_data.GetSpeed() * Time.deltaTime);
    }

    // Destroy bullet when it hits player
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}