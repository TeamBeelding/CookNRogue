using System;
using UnityEngine;

namespace Enemy.Data
{
    [CreateAssetMenu(fileName = "TBHData", menuName = "Enemy/TBHData")]
    public class TBHData : ScriptableObject
    {
        [SerializeField] private float _attackRange;
        [SerializeField] private float _attackSpeed;
        [SerializeField] private float _angleShot;
        [SerializeField] private int _numberOfBullet;
        [SerializeField] private float _damagePerBullet;
        [SerializeField] private float _health;
        [SerializeField] private float _teleportRange;
        [SerializeField] private float _minimumRadius;
        [SerializeField] private float _delayBetweenTeleport;

        private void Reset()
        {
            _attackRange = 8f;
            _attackSpeed = 2f;
            _angleShot = 30f;
            _numberOfBullet = 3;
            _damagePerBullet = 2f;
            _health = 5f;
            _teleportRange = 8f;
            _minimumRadius = 6f;
            _delayBetweenTeleport = 2f;
        }

        public float AttackRange => _attackRange;
        public float AttackSpeed => _attackSpeed;
        public float AngleShot => _angleShot;
        public int NumberOfBullet => _numberOfBullet;
        public float DamagePerBullet => _damagePerBullet;
        public float Health => _health;
        public float TeleportRange => _teleportRange; 
        public float MinimumRadius => _minimumRadius;
        public float DelayBetweenTeleport => _delayBetweenTeleport;
    }
}