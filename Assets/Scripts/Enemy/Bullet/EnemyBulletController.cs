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
    private float _damage;
    
    private void Start()
    {
        _speed = _data.GetSpeed();
        _damage = _data.GetDamage();
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
    
    public void SetDirection(Vector3 dir)
    {
        _direction = new Vector3(dir.x - transform.position.x, 0, dir.z - transform.position.z);
        _direction.Normalize();
        isDirectionSet = true;
    }
    
    public void SetDamage(float damage)
    {
        _damage = damage;
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
            other.GetComponent<PlayerController>().TakeDamage(_damage); 
            Destroy(gameObject);
        }

        if (!other.transform.parent.CompareTag("Enemy"))
            Destroy(gameObject);
    }
}