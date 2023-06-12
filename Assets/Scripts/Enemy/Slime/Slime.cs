using Enemy.Data;
using Enemy.Effect_And_Juiciness;
using Enemy.Minimoyz;
using NaughtyAttributes;
using Newtonsoft.Json.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Enemy.Slime
{
    public class Slime : EnemyController
    {
        [SerializeField] private SlimeData data;
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private GameObject gun;
        [SerializeField, Required("Prefabs minimoyz")] private GameObject minimoyz;
        [FormerlySerializedAs("_minimoyzSpawnChecker")] [SerializeField, Required("Minimoyz spawn checker")] private CheckingSpawn spawnChecker;

        private Animator animator;

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
            Healthpoint = data.GetHealth;
            agent = GetComponent<NavMeshAgent>();
            agent.speed = data.GetSpeed;
            agent.stoppingDistance = data.GetAttackRange;
            spawnChecker = GetComponentInChildren<CheckingSpawn>();
        }

        protected override void Awake()
        {
            base.Awake();
        
            Healthpoint = data.GetHealth;
            agent = GetComponent<NavMeshAgent>();
            agent.speed = data.GetSpeed;
            agent.stoppingDistance = data.GetAttackRange;
        }

        // Start is called before the first frame update
        protected override void Start()
        {
            state = data.GetFocusPlayer ? State.Chase : State.Neutral;
            spawnChecker = GetComponentInChildren<CheckingSpawn>();
            animator = GetComponentInChildren<Animator>();
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
            animator.SetBool("isAttack", _canAttackAnim);

            switch (state)
            {
                case State.Neutral:
                    animator.SetBool("isWalking", false);
                    animator.SetBool("isAttack", false);
                    break;
                case State.Chase:
                    Chase();
                    animator.SetBool("isWalking", true);
                    animator.SetBool("isAttack", false);
                    break;
                case State.KeepingDistance:
                    animator.SetBool("isWalking", false);
                    animator.SetBool("isAttack", false);
                    KeepDistance();
                    break;
                case State.Attack:
                    animator.SetBool("isWalking", false);
                    //animator.SetBool("isAttack", true);
                    Attack(ThrowMinimoyz, data.GetAttackSpeed);
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
        
            if (Vector3.Distance(transform.position, Player.transform.position) <= data.GetFocusRange)
            {
                FocusPlayer = true;
                state = State.Chase;
            }
            else
            {
                state = State.Neutral;
            }

            if (Vector3.Distance(transform.position, Player.transform.position) <= data.GetAttackRange)
            {
                if (Vector3.Distance(transform.position, Player.transform.position) <= data.GetMinimumDistanceToKeep)
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
            if (Vector3.Distance(transform.position, Player.transform.position) <= data.GetMinimumDistanceToKeep)
            {
                Vector3 target = transform.position - (Player.transform.position - transform.position);
                agent.stoppingDistance = 0;
                agent.SetDestination(target);
            }
        }
    
        protected override void Chase()
        {
            agent.stoppingDistance = data.GetAttackRange;
            agent.SetDestination(Player.transform.position);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void ThrowMinimoyz()
        {
            Transform point = RandomPoint();
            
            spawnChecker.SetTransformPosition(point.position);
            
            if (!spawnChecker.IsPathValid(Player.transform.position))
                ThrowMinimoyz();

            GameObject minimoyz = Instantiate(this.minimoyz, gun.transform.position, quaternion.identity);
            minimoyz.GetComponent<MinimoyzController>().SetIsThrowing(true);
            minimoyz.GetComponent<ThrowingEffect>().ThrowMinimoyz(point, data.GetThrowingMaxHeight, data.GetThrowingSpeed);
        }

        private Transform RandomPoint()
        {
            Vector3 center = Player.transform.position;
            float distanceBetweenRadius = data.GetOuterRadius - data.GetInnerRadius;
            float randomRadius = Random.value;
            float distanceFromCenter = data.GetInnerRadius + (randomRadius * distanceBetweenRadius);
            
            float randomAngle = Random.Range(0, 360);
            float angleInRadians = randomAngle * Mathf.Deg2Rad;
            
            float x = center.x + (distanceFromCenter * Mathf.Cos(angleInRadians));
            float y = center.y;
            float z = center.z + (distanceFromCenter * Mathf.Sin(angleInRadians));

            Vector3 position = new Vector3(x, y, z);

            GameObject pointObject = new GameObject("RandomPoint");
            pointObject.transform.position = position;

            return pointObject.transform;
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

            for (int i = 0; i < data.GetSlimeSpawnWhenDying; i++)
            {
                Vector2 origin = new Vector2(transform.position.x, transform.position.z);
                Vector3 point = Random.insideUnitCircle * data.GetRadiusMinimoyzSpawnPoint + origin;
                point = new Vector3(point.x, 0, point.y);
            
                Instantiate(minimoyz, point, Quaternion.identity);
            }
        
            base.Dying();
        }

        public override bool IsMoving()
        {
            throw new System.NotImplementedException();
        }
    }
}
