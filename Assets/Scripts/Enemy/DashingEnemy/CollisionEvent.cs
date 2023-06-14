using System;
using UnityEngine;

namespace Enemy.DashingEnemy
{
    public class CollisionEvent : MonoBehaviour
    {
        [SerializeField]
        private AK.Wwise.Event _Play_SFX_Cabbage_Charge_Impact;
        private ChargingEnemy _chargingEnemy;

        private void Awake()
        {
            _chargingEnemy = GetComponent<ChargingEnemy>();
        }

        private void Reset()
        {
            _chargingEnemy = GetComponent<ChargingEnemy>();
        }

        private void OnTriggerEnter(Collider other)
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
}
