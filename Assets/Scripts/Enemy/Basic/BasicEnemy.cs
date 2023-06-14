using System;
using System.Collections;
using Enemy.Data;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;
using UnityEngine.Serialization;

namespace Enemy.Basic
{
    public class BasicEnemy : EnemyController
    {[Header("Sound")]
        [SerializeField]
        private AK.Wwise.Event _Play_Weapon_Hit;
        [SerializeField]
        private AK.Wwise.Event _Play_SFX_Corn_Death;
        [SerializeField]
        private AK.Wwise.Event _Play_SFX_Corn_Hit;
        [SerializeField]
        private AK.Wwise.Event _Play_SFX_Corn_Footsteps;
        [SerializeField]
        private AK.Wwise.Event _Play_SFX_Corn_Attack_Charge;
        [SerializeField]
        private AK.Wwise.Event _Play_SFX_Corn_Attack_Shot;
    
        [SerializeField] private EnemyData data;
        [SerializeField] private NavMeshAgent agent;

        [SerializeField] private GameObject m_gun;
        [SerializeField] private GameObject m_bullet;
        [SerializeField] private ParticleSystem m_stateSystem;
        [SerializeField] private GameObject physics;

        private Coroutine _stateCoroutine;

        [SerializeField]
        private Animator animator;
        public enum State
        {
            Neutral,
            Chase,
            Attack,
            Dying,
        }

        [SerializeField] private State state;

        protected override void Awake()
        {
            base.Awake();

            agent = GetComponent<NavMeshAgent>();
            agent.speed = data.GetSpeed;
            agent.stoppingDistance = data.GetAttackRange;
            FocusPlayer = data.GetFocusPlayer;
            Healthpoint = data.GetHealth;

            animator = GetComponentInChildren<Animator>();
        }

        // Start is called before the first frame update
        protected override void Start()
        {
            SetState(FocusPlayer ? State.Chase : State.Neutral);

            base.Start();
        }

        protected override void Update()
        {
            base.Update();

            if (state == State.Dying)
                return;

            AreaDetection();
        }

        private void Reset()
        {
            Healthpoint = data.GetHealth;
            agent = GetComponent<NavMeshAgent>();
            agent.speed = data.GetSpeed;
            agent.stoppingDistance = data.GetAttackRange;
        }

        private State GetState()
        {
            return state;
        }

        private void SetState(State value)
        {
            if (_stateCoroutine != null)
                StopCoroutine(_stateCoroutine);
            
            state = value;

            StateManagement();
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void StateManagement()
        {
            switch (state)
            {
                case State.Neutral:
                    animator.SetBool("isWalking", false);
                    animator.SetBool("isAttack", false);
                    break;
                case State.Chase:
                    animator.SetBool("isWalking", true);
                    animator.SetBool("isAttack", false);
                    Chase();
                    break;
                case State.Attack:
                    animator.SetBool("isWalking", false);
                    animator.SetBool("isAttack", false);
                    transform.LookAt(Player.transform.position);
                    Attack(Shot, data.GetAttackSpeed);
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

            if (FocusPlayer)
            {
                if (Vector3.Distance(transform.position, Player.transform.position) <= data.GetRangeDetection)
                    if (Vector3.Distance(transform.position, Player.transform.position) > data.GetAttackRange)
                        SetState(State.Chase);
                    else
                        SetState(State.Attack);
            }
            else
            {
                if (Vector3.Distance(transform.position, Player.transform.position) <= data.GetRangeDetection)
                    FocusPlayer = true;
                else 
                    SetState(State.Neutral);
            }
        }

        protected override void Chase()
        {
            if (Player.GetComponent<PlayerController>().GetIsOnTutorial())
                return;
            
            if (state == State.Dying)
                return;

            _stateCoroutine = StartCoroutine(IChase());

            IEnumerator IChase()
            {
                while (state == State.Chase)
                {
                    if (Vector3.Distance(transform.position, Player.transform.position) <= data.GetAttackRange)
                    {
                        SetState(State.Attack);
                        _stateCoroutine = null;
                    }

                    agent.SetDestination(Player.transform.position);

                    yield return null;
                }
            }
        }

        private void Shot()
        {
            GameObject shot = Instantiate(m_bullet, m_gun.transform.position, Quaternion.identity);
            shot.GetComponent<EnemyBulletController>().SetDirection(Player.transform);
            animator.SetBool("isAttack", true);
            _Play_SFX_Corn_Attack_Shot.Post(gameObject);
        }

        protected override void Dying()
        {
            physics.SetActive(false);

            StopAllCoroutines();
            animator.SetBool("isDead", true);

            StartCoroutine(IDeathAnim());

            IEnumerator IDeathAnim()
            {
                yield return new WaitForSeconds(2f);
                base.Dying();
                m_stateSystem.gameObject.SetActive(false);
            }
        }

        public override bool IsMoving()
        {
            return state == State.Chase;
        }

        public override void TakeDamage(float damage = 1, bool isCritical = false)
        {
            base.TakeDamage(damage, isCritical);

            if (state == State.Neutral) 
            {
                SetState(State.Chase);
                _Play_SFX_Corn_Hit.Post(gameObject);
                _Play_Weapon_Hit.Post(gameObject);
            }

            if (Healthpoint <= 0)
            {

                _Play_SFX_Corn_Death.Post(gameObject);
                SetState(State.Dying);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, data.GetRangeDetection);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, data.GetAttackRange);
        }
    }
}