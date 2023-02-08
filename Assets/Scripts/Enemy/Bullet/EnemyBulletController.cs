using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletController : MonoBehaviour
{
    [SerializeField]
    private EnemyBulletData _data;
    private bool isDirectionSet = false;
    private float _speed;
    private Vector3 _direction;
    
    private void Start()
    {
        _speed = _data.GetSpeed();
        Destroy(gameObject, _data.GetLifeTime());
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
        _direction = new Vector3(dir.position.x - transform.position.x, 0, dir.position.z - transform.position.z);
        _direction.Normalize();
        isDirectionSet = true;
    }

    // Move bullet
    private void Move()
    {
        transform.position += _direction * (_data.GetSpeed() * Time.deltaTime);
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