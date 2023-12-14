using System.Collections;
using Enemy.Data;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy.Minimoyz
{
    public class MinimoyzController : EnemyController
    {
        [Header("Sound")]
        [SerializeField]
        private AK.Wwise.Event _Play_SFX_Pea_Footsteps;
        [SerializeField]
        private AK.Wwise.Event _Play_SFX_Pea_Spawn;
        [SerializeField]
        private AK.Wwise.Event _Play_SFX_Pea_Death;
        [SerializeField]
        private AK.Wwise.Event _Play_Weapon_Hit;
        [SerializeField]
        private AK.Wwise.Event _Play_SFX_Pea_Movement;
        [SerializeField]
        private AK.Wwise.Event _Stop_SFX_Pea_Movement;

        [SerializeField] private MinimoyzData data;
        [SerializeField] private SlimeData slimeData;
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private GameObject physicsMinimoyz;

        private Coroutine coroutineState;
        private NavMeshPath navMeshPath;
    
        public enum State
        {
            Neutral,
            Chase,
            Cast,
            Attack,
            ChaseAndAttack,
            Dying,
        }
    
        public State state;

        protected override void Awake()
        {
            base.Awake();
            navMeshPath = new NavMeshPath();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            agent = GetComponent<NavMeshAgent>();

            agent.speed = data.GetSpeed();
            agent.stoppingDistance = data.GetAttackRange();
            FocusPlayer = data.GetFocusPlayer();
            Healthpoint = data.GetHealth();

            SetState(State.Chase);
        }

        // Update is called once per frame
        protected override void Update()
        {
            if (!gameObject.activeInHierarchy)
                return;

            if (state == State.Dying)
                return;
        
            AreaDetection();

            base.Update();
        }

        public void SetFocus(bool value = true) => FocusPlayer = value;

        private void StateManagement()
        {
            switch (state)
            {
                case State.Neutral:
                    break;
                case State.Chase:
                    Chase();
                    _Play_SFX_Pea_Movement.Post(gameObject);
                    break;
                case State.Cast:
                    Cast();
                    break;
                case State.Attack:
                    Attack(Attack, data.GetAttackSpeed());
                    _Stop_SFX_Pea_Movement.Post(gameObject);
                    break;
                case State.ChaseAndAttack:
                    ChaseAndAttack();
                    _Stop_SFX_Pea_Movement.Post(gameObject);
                    break;
                case State.Dying:
                    _Stop_SFX_Pea_Movement.Post(gameObject);
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

            if (Vector3.Distance(transform.position, Player.transform.position) > data.GetAttackRange())
                SetState(State.Chase);
            else
                SetState(State.Attack);
        }

        public State GetState() => state;

        public void SetState(State value)
        {
            if (coroutineState != null)
                StopCoroutine(coroutineState);
            
            state = value;
            
            StateManagement();
        }

        public override bool IsMoving() => throw new System.NotImplementedException();

        private void Cast()
        {
            if (state == State.Dying)
                return;
        
            coroutineState = StartCoroutine(CastAttack());
            
            IEnumerator CastAttack()
            {
                yield return new WaitForSeconds(0.5f);
                SetState(State.Attack);
            }
        }

        private void Attack()
        {
            if (state == State.Dying)
                return;
            
            HitPlayer();
        }

        private void HitPlayer()
        {
            Player.GetComponent<PlayerController>().TakeDamage(data.GetDamage());
            SetState(State.Cast);
        }

        protected override void Chase()
        {
            if (!gameObject.activeSelf)
                return;

            if (!physicsMinimoyz.activeSelf)
                physicsMinimoyz.SetActive(true);
 
            coroutineState = StartCoroutine(IChase());

            IEnumerator IChase()
            {
                while (state == State.Chase)
                {
                    agent.SetDestination(Player.transform.position);

                    if (!agent.CalculatePath(Player.transform.position, navMeshPath))
                    {
                        SetState(State.Dying);
                    }
                    else
                    {
                        switch (navMeshPath.status)
                        {
                            case NavMeshPathStatus.PathPartial:
                            case NavMeshPathStatus.PathInvalid:
                                SetState(State.Dying);
                                break;
                        }
                    }

                    yield return null;
                }
            }
        }
    
        private void ChaseAndAttack()
        {
            coroutineState = StartCoroutine(IChasingAndAttacking());
            
            IEnumerator IChasingAndAttacking()
            {
                while (state == State.ChaseAndAttack)
                {  
                    agent.SetDestination(Player.transform.position);
                    
                    if (Vector3.Distance(transform.position, Player.transform.position) <= data.GetAttackRange())
                        HitPlayer();
                    
                    yield return null;
                }
            }
        }
    
        public override void TakeDamage(float damage = 1, bool isCritical = false)
        {
            base.TakeDamage(damage, isCritical);
            
            _Play_SFX_Pea_Death.Post(gameObject);
            _Play_Weapon_Hit.Post(gameObject);

            if (Healthpoint <= 0)
            {
                _Play_SFX_Pea_Death.Post(gameObject);
                _Play_Weapon_Hit.Post(gameObject);
                SetState(State.Dying);
            }
        }

        protected override void Dying()
        {
            waveManager?.SlowMotion();

            base.Dying();
        }
    }
}
