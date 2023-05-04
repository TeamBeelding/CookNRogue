using System;
using UnityEngine;

public class CollisionEvent : MonoBehaviour
{
    private ChargingEnemy _chargingEnemy;

    private void Awake()
    {
        _chargingEnemy = GetComponentInParent<ChargingEnemy>();
    }

    private void Reset()
    {
        _chargingEnemy = GetComponentInParent<ChargingEnemy>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _chargingEnemy.CollideWithPlayer();
        }
        
        if (other.gameObject.CompareTag("Obstruction"))
        {
            _chargingEnemy.CollideWithObstruction();
        }
    }
}
