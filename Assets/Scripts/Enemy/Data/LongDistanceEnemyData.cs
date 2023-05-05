using UnityEngine;

namespace Enemy.Data
{
    [CreateAssetMenu(fileName = "LongDistanceEnemyData", menuName = "Enemy/LongDistanceEnemyData")]
    public class LongDistanceEnemyData : ScriptableObject
    {
        [SerializeField] private float speed = 4f;
        [SerializeField] private float attackRange = 10f;
        [SerializeField] private float attackSpeed = 2f;
        [SerializeField] private float health = 10f;
        [SerializeField] private float minimumDistanceToKeep = 8f;
        [SerializeField] private int numberShootBeforeMoving = 3;
        
        public float AttackRange => attackRange;
        public float Speed => speed;
        public float AttackSpeed => attackSpeed;
        public float Health => health;
        public float MinimumDistanceToKeep => minimumDistanceToKeep;    
        public int NumberShootBeforeMoving => numberShootBeforeMoving;
    }
}
