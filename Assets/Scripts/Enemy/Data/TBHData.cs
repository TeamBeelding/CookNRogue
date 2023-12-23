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

        [Space()]
        [Header("Bullet")]

        [SerializeField] private float minSpread = -2f;
        [SerializeField] private float maxSpread = 2f;

        [Space()]
        [Header("Between each bullet")]
        [SerializeField] private float minSpeed = 0.1f;
        [SerializeField] private float maxSpeed = 0.5f;

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
            minSpread = -2f;
            maxSpread = 2f;
            minSpeed = 0.1f;
            maxSpeed = 0.5f;
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
        public float GetMinSpread => minSpread;
        public float GetMaxSpread => maxSpread;
        public float GetMaxSpeed => maxSpeed;
        public float GetMinSpeed => minSpeed;
    }
}
