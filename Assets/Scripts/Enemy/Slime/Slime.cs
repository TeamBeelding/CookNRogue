using NaughtyAttributes;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Enemy.Slime
{
    public class Slime : EnemyController
    {
        [SerializeField] private SlimeData _data;
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private GameObject _gun;
        [SerializeField, Required("Prefabs minimoyz")] private GameObject _minimoyz;
    
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
            _agent = GetComponent<NavMeshAgent>();
            healthpoint = _data.GetHealth;
            _agent = GetComponent<NavMeshAgent>();
            _agent.speed = _data.GetSpeed;
            _agent.stoppingDistance = _data.GetAttackRange;
        }

        protected override void Awake()
        {
            base.Awake();
        
            healthpoint = _data.GetHealth;
            _agent = GetComponent<NavMeshAgent>();
            _agent.speed = _data.GetSpeed;
            _agent.stoppingDistance = _data.GetAttackRange;
        }

        // Start is called before the first frame update
        protected override void Start()
        {
            state = _data.GetFocusPlayer ? State.Chase : State.Neutral;

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
        
            if (Vector3.Distance(transform.position, player.transform.position) <= _data.GetFocusRange)
            {
                _focusPlayer = true;
                state = State.Chase;
            }
            else
            {
                state = State.Neutral;
            }

            if (Vector3.Distance(transform.position, player.transform.position) <= _data.GetAttackRange)
            {
                if (Vector3.Distance(transform.position, player.transform.position) <= _data.GetMinimumDistanceToKeep)
                    state = State.KeepingDistance;
                else
                    state = State.Attack;
            }
            else
            {
                if (_focusPlayer)
                    state = State.Chase;
            }
        }
    
        private void KeepDistance()
        {
            if (Vector3.Distance(transform.position, player.transform.position) <= _data.GetMinimumDistanceToKeep)
            {
                Vector3 target = transform.position - (player.transform.position - transform.position);
                _agent.stoppingDistance = 0;
                _agent.SetDestination(target);
            }
        }
    
        protected override void Chase()
        {
            _agent.stoppingDistance = _data.GetAttackRange;
            _agent.SetDestination(player.transform.position);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void ThrowMinimoyz()
        {
            GameObject minimoyz = Instantiate(_minimoyz, RandomPoint(), quaternion.identity);
            minimoyz.GetComponent<MinimoyzController>().SetFocus();
            Debug.Log("Throw Minimoyz");
        }

        private Vector3 RandomPoint()
        {
            Vector3 center = player.transform.position;
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
    
        public override void TakeDamage(float damage)
        {
            base.TakeDamage(damage);
        
            if (healthpoint <= 0)
            {
                state = State.Dying;
            }
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
