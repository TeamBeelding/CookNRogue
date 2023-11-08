using System.Collections;
using Enemy.Data;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy.LongDistanceEnemy
{
    public class LongDistanceEnemy : EnemyController
    {
        [SerializeField] private LongDistanceEnemyData _data;
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private GameObject _gun;
        [SerializeField] private GameObject _bullet;
        [SerializeField] private ParticleSystem m_stateSystem;
        
        private int _numberShootBeforeMoving;
        
        private Coroutine _stateCoroutine;

        public enum State
        {
            Chasing,
            Attacking,
            MovingAnotherPosition,
            TakingDistance,
            Dying
        }
    
        public State _state;

        protected override void Awake()
        {
            base.Awake();
        
            _agent = GetComponent<NavMeshAgent>();
            _agent.speed = _data.Speed;
            _agent.stoppingDistance = _data.AttackRange;
            Healthpoint = _data.Health;
            _numberShootBeforeMoving = _data.NumberShootBeforeMoving;
        }
        
        private void Reset()
        {
            Healthpoint = _data.Health;
            _agent = GetComponent<NavMeshAgent>();
            _agent.speed = _data.Speed;
            _agent.stoppingDistance = _data.AttackRange;
        }

        private State GetState() => _state;
    
        private void SetState(State value)
        {
            _stateCoroutine = null;
            _state = value;

            StateManagement();
        }

        private void StateManagement()
        {
            switch (_state)
            {
                case State.Chasing:
                    Chasing();
                    break;
                case State.Attacking:
                    Attack(Shot, _data.AttackSpeed);
                    break;
                case State.MovingAnotherPosition:
                    MovingAnotherPosition();
                    break;
                case State.TakingDistance:
                    TakingDistance();
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
            if (Vector3.Distance(transform.position, Player.transform.position) > _data.AttackRange)
                SetState(State.Chasing);
            else
            {
                if (Vector3.Distance(transform.position, Player.transform.position) < _data.MinimumDistanceToKeep)
                    SetState(State.TakingDistance);
                else if (Vector3.Distance(transform.position, Player.transform.position) <= _data.AttackRange)
                    SetState(State.Attacking);
            }
        }

        private void Chasing()
        {
            _stateCoroutine = StartCoroutine(IChasing());
            
            IEnumerator IChasing()
            {
                while (GetState() == State.Chasing)
                {
                    _agent.SetDestination(Player.transform.position);
                    yield return null;
                }
            }
        }
        
        private void Shot()
        {
            AreaDetection();
            
            _numberShootBeforeMoving--;

            GameObject shot = Instantiate(_bullet, _gun.transform.position, Quaternion.identity);
            shot.GetComponent<EnemyBulletController>().SetDirection(Player.transform);

            if (_numberShootBeforeMoving == 0)
                SetState(State.MovingAnotherPosition);
        }
        
        /// <summary>
        /// He will move to another place where he is still in range to shoot another time
        /// </summary>
        private void MovingAnotherPosition()
        {
            _numberShootBeforeMoving = _data.NumberShootBeforeMoving;
            
            Vector3 newPosition = GetRandomPoint();
            
            if (CanMoveThere(newPosition))
                _stateCoroutine = StartCoroutine(IMovingAnotherPosition());
            else
                MovingAnotherPosition();

            IEnumerator IMovingAnotherPosition()
            {
                while (Vector3.Distance(transform.position, newPosition) > 0.5f)
                {
                    _agent.SetDestination(newPosition);
                    yield return null;
                }
            }
            
            Vector3 GetRandomPoint()
            {
                Vector3 center = Player.transform.position;
                Vector3 randomPoint = center + Random.insideUnitSphere * _data.AttackRange;

                return randomPoint;
            }
            
            bool CanMoveThere(Vector3 position)
            {
                if (Physics.Raycast(position, Player.transform.position.normalized, out RaycastHit hit, _data.AttackRange))
                {
                    if (hit.collider.CompareTag("Obstruction"))
                        return false;
                }
                
                return true;
            }
        }
        
        /// <summary>
        /// He will run until he reaches the farthest extremities of his range
        /// </summary>
        private void TakingDistance()
        {
            Vector3 randomPoint = Player.transform.position + Random.insideUnitSphere;
            Vector3 position = randomPoint * _data.AttackRange;

            _stateCoroutine = StartCoroutine(ITakeDistance());
            
            IEnumerator ITakeDistance()
            {
                while (Vector3.Distance(transform.position, position) > 0.5f)
                {
                    Debug.Log("taking distance");
                    _agent.SetDestination(position);
                    yield return null;
                }
            }
        }

        public override bool IsMoving() => false;
        
        public override void TakeDamage(float damage = 1, bool isCritical = false)
        {
            base.TakeDamage(damage, isCritical);

            if (Healthpoint <= 0)
                SetState(State.Dying);
        }
    }
}