using System;
using NaughtyAttributes;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Enemy.Slime
{
    public class Slime : EnemyController
    {
        [SerializeField] private SlimeData _data;
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField, Required("Prefabs minimoyz")] private GameObject _minimoyz;
        [FormerlySerializedAs("_minimoyzSpawnChecker")] [SerializeField, Required("Minimoyz spawn checker")] private CheckingSpawn spawnChecker;

        public enum State
        {
            Neutral,
            KeepingDistance,
            Chase,
            Attack,
            Dying,
        }
    
        public State state;

        private void Reset()
        {
            Healthpoint = _data.GetHealth;
            _agent = GetComponent<NavMeshAgent>();
            _agent.speed = _data.GetSpeed;
            _agent.stoppingDistance = _data.GetAttackRange;
            spawnChecker = GetComponentInChildren<CheckingSpawn>();
        }

        protected override void Awake()
        {
            base.Awake();
        
            Healthpoint = _data.GetHealth;
            _agent = GetComponent<NavMeshAgent>();
            _agent.speed = _data.GetSpeed;
            _agent.stoppingDistance = _data.GetAttackRange;
        }

        // Start is called before the first frame update
        protected override void Start()
        {
            state = _data.GetFocusPlayer ? State.Chase : State.Neutral;
            spawnChecker = GetComponentInChildren<CheckingSpawn>();

            base.Start();
        }

        // Update is called once per frame
        protected override void Update()
        {
            StateManagement();
        }

        private void FixedUpdate()
        {
            if (state == State.Dying)
                return;
        
            AreaDetection();
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void StateManagement()
        {
            switch (state)
            {
                case State.Neutral:
                    break;
                case State.Chase:
                    Chase();
                    break;
                case State.KeepingDistance:
                    KeepDistance();
                    break;
                case State.Attack:
                    Attack(ThrowMinimoyz, _data.GetAttackSpeed);
                    break;
                case State.Dying:
                    Dying();
                    break;
                default:
                    Dying();
                    break;
            }
        }

        private void AreaDetection()
        {
            if (state == State.Dying)
                return;
        
            if (Vector3.Distance(transform.position, Player.transform.position) <= _data.GetFocusRange)
            {
                FocusPlayer = true;
                state = State.Chase;
            }
            else
            {
                state = State.Neutral;
            }

            if (Vector3.Distance(transform.position, Player.transform.position) <= _data.GetAttackRange)
            {
                if (Vector3.Distance(transform.position, Player.transform.position) <= _data.GetMinimumDistanceToKeep)
                    state = State.KeepingDistance;
                else
                    state = State.Attack;
            }
            else
            {
                if (FocusPlayer)
                    state = State.Chase;
            }
        }
    
        private void KeepDistance()
        {
            if (Vector3.Distance(transform.position, Player.transform.position) <= _data.GetMinimumDistanceToKeep)
            {
                Vector3 target = transform.position - (Player.transform.position - transform.position);
                _agent.stoppingDistance = 0;
                _agent.SetDestination(target);
            }
        }
    
        protected override void Chase()
        {
            _agent.stoppingDistance = _data.GetAttackRange;
            _agent.SetDestination(Player.transform.position);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void ThrowMinimoyz()
        {
            Vector3 point = RandomPoint();
            
            spawnChecker.SetTransformPosition(point);
            
            if (!spawnChecker.IsPathValid(Player.transform.position))
                ThrowMinimoyz();

            GameObject minimoyz = Instantiate(_minimoyz, point, quaternion.identity);
            minimoyz.GetComponent<MinimoyzController>().SetFocus();
        }

        private Vector3 RandomPoint()
        {
            Vector3 center = Player.transform.position;
            float distanceBetweenRadius = _data.GetOuterRadius - _data.GetInnerRadius;
            float randomRadius = Random.value;
            float distanceFromCenter = _data.GetInnerRadius + (randomRadius * distanceBetweenRadius);
            
            float randomAngle = Random.Range(0, 360);
            float angleInRadians = randomAngle * Mathf.Deg2Rad;
            
            float x = center.x + (distanceFromCenter * Mathf.Cos(angleInRadians));
            float y = center.y;
            float z = center.z + (distanceFromCenter * Mathf.Sin(angleInRadians));

            return new Vector3(x, y, z);
        }
    
        public override void TakeDamage(float damage = 1, bool isCritical = false)
        {
            base.TakeDamage(damage, isCritical);
        
            if (Healthpoint <= 0)
            {
                state = State.Dying;
            }
        }

        private void SetState(State value)
        {
            state = value;
        }

        protected override void Dying()
        {
            for (int i = 0; i < _data.GetSlimeSpawnWhenDying; i++)
            {
                Vector2 origin = new Vector2(transform.position.x, transform.position.z);
                Vector3 point = Random.insideUnitCircle * _data.GetRadiusMinimoyzSpawnPoint + origin;
                point = new Vector3(point.x, 0, point.y);
            
                Instantiate(_minimoyz, point, Quaternion.identity);
            }
        
            base.Dying();
        }

        public override bool IsMoving()
        {
            throw new System.NotImplementedException();
        }
    }
}
