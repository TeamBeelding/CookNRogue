using Enemy.Data;
using Enemy.Effect_And_Juiciness;
using Enemy.Minimoyz;
using NaughtyAttributes;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using System.Collections;
using Random = UnityEngine.Random;

namespace Enemy.Slime
{
    public class Slime : EnemyController
    {   [Header("Sound")]
        [SerializeField]
        private AK.Wwise.Event _Play_SFX_Pea_Pod_Footsteps;
        [SerializeField]
        private AK.Wwise.Event _Stop_SFX_Pea_Pod_Footsteps;
        [SerializeField]
        private AK.Wwise.Event _Play_SFX_Pea_Pod_Hit;
        [SerializeField]
        private AK.Wwise.Event _Play_Weapon_Hit;
        [SerializeField]
        private AK.Wwise.Event _Play_SFX_Pea_Pod_Death;
        [SerializeField]
        private AK.Wwise.Event _Play_SFX_Pea_Spawn;

        [Space]

        [SerializeField] private SlimeData data;
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private GameObject gun;
        [SerializeField, Required("Prefabs minimoyz")] private GameObject minimoyz;
        [FormerlySerializedAs("_minimoyzSpawnChecker")] [SerializeField, Required("Minimoyz spawn checker")] private CheckingSpawn spawnChecker;

        private Animator animator;
        private Coroutine stateCoroutine;
        [SerializeField] private GameObject physics;

        public enum State
        {
            Neutral,
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

            if (!animator)
                animator = GetComponent<Animator>();
        }

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            SetState(data.GetFocusPlayer ? State.Chase : State.Neutral);
            spawnChecker = GetComponentInChildren<CheckingSpawn>();
            animator = GetComponentInChildren<Animator>();
        }

        private void FixedUpdate()
        {
            if (state == State.Dying)
                return;

            AreaDetection();
        }
        
        private void SetState(State value)
        {
            if (stateCoroutine != null)
                stateCoroutine = null;
         
            state = value;
            
            StateManagement();
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
                    _Stop_SFX_Pea_Pod_Footsteps.Post(gameObject);
                    break;
                case State.Chase:
                    Chase();
                    animator.SetBool("isWalking", true);
                    animator.SetBool("isAttack", false);
                    _Play_SFX_Pea_Pod_Footsteps.Post(gameObject);
                    break;
                case State.Attack:
                    animator.SetBool("isWalking", false);
                    _Stop_SFX_Pea_Pod_Footsteps.Post(gameObject);
                    transform.LookAt(Player.transform.position);
                    animator.SetBool("isAttack", true);
                    Attack(ThrowMinimoyz, data.GetAttackSpeed);
                    break;
                case State.Dying:
                    Dying();
                    break;
                default:
                    Dying();
                    break;
                    //_Stop_SFX_Pea_Pod_Footsteps.IsValid
            }
        }

        private void AreaDetection()
        {
            if (state == State.Dying)
                return;

            if (!Player)
            {
                Debug.Log("no player");
                return;
            }

            if (Vector3.Distance(transform.position, Player.transform.position) > data.GetAttackRange)
                SetState(State.Chase);
            else
                SetState(State.Attack);
        }
    
        protected override void Chase()
        {
            agent.stoppingDistance = data.GetAttackRange;
            stateCoroutine = StartCoroutine(IChase());

            IEnumerator IChase()
            {
                while (state == State.Chase)
                {
                    agent.SetDestination(Player.transform.position);
                    
                    yield return null;
                }
            }
        }

        // Todo : Throw a sprite instead a prefabs and instantiate prefabs on position
        // Todo : Pooling Sprite and AI
        private void ThrowMinimoyz()
        {
            Vector3 point = RandomPoint();
            
            spawnChecker.SetTransformPosition(point);
            
            if (!spawnChecker.CanThrowHere())
                return;
                     
            GameObject minimoyz = Instantiate(this.minimoyz, gun.transform.position, quaternion.identity);
            //minimoyz.GetComponent<MinimoyzController>().SetIsThrowing(true);
            minimoyz.GetComponent<ThrowingEffect>().ThrowMinimoyz(point, data.GetThrowingMaxHeight, data.GetThrowingSpeed);
            
            if (_canAttackAnim)
                _Play_SFX_Pea_Spawn.Post(minimoyz);
        }

        private Vector3 RandomPoint()
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
            
            return position;
        }

        public override void TakeDamage(float damage = 1, bool isCritical = false)
        {
            base.TakeDamage(damage, isCritical);
            _Play_SFX_Pea_Pod_Hit.Post(gameObject);
            _Play_Weapon_Hit.Post(gameObject);

            if (Healthpoint <= 0)
            {
                state = State.Dying;
                _Play_SFX_Pea_Pod_Death.Post(gameObject);
            }
        }

        // Todo : Add fix for spawning minimoyz inside circle with some distance between them
        // Maybe Activate physics
        protected override void Dying()
        {
            physics.SetActive(false);
            agent.SetDestination(transform.position);

            animator.SetBool("isDead", true);

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

# if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, 2);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, data.GetInnerRadius);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, data.GetOuterRadius);
        }
    }
# endif

}
