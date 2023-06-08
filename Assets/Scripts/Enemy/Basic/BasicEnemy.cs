using System;
using System.Collections;
using Enemy.Data;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy.Basic
{
    public class BasicEnemy : EnemyController
    {
        [SerializeField]
        private EnemyData data;
        [SerializeField]
        private NavMeshAgent _agent;
    
        [SerializeField]
        private GameObject m_gun;
        [SerializeField]
        private GameObject m_bullet;
        [SerializeField]
        private ParticleSystem m_stateSystem;
        [SerializeField]
        private Renderer stateRenderer;

        private Coroutine _chaseCoroutine;
        [SerializeField]
        private Animator animator;
        public enum State
        {
            Neutral,
            Chase,
            Attack,
            Dying,
        }

        [SerializeField]
        private State state;
    
        protected override void Awake()
        {
            base.Awake();
        
            _agent = GetComponent<NavMeshAgent>();
            _agent.speed = data.GetSpeed;
            _agent.stoppingDistance = data.GetAttackRange;
            FocusPlayer = data.GetFocusPlayer;
            Healthpoint = data.GetHealth;
            animator = GetComponentInChildren<Animator>();
            stateRenderer = m_stateSystem.GetComponent<Renderer>();
        }

        // Start is called before the first frame update
        protected override void Start()
        {
            SetState(FocusPlayer ? State.Chase : State.Neutral);
            base.Start();
            
        }

        private void FixedUpdate()
        {
            if (state == State.Dying)
                return;
        
            AreaDetection();
        }
        
        private void Reset()
        {
            Healthpoint = data.GetHealth;
            _agent = GetComponent<NavMeshAgent>();
            _agent.speed = data.GetSpeed;
            _agent.stoppingDistance = data.GetAttackRange;
        }

        private State GetState()
        {
            return state;
        }

        private void SetState(State value)
        {
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

            if (Vector3.Distance(transform.position, Player.transform.position) <= data.GetRangeDetection)
            {
                FocusPlayer = true;
            }

            if (FocusPlayer)
            {
                transform.LookAt(Player.transform);
                if (Vector3.Distance(transform.position, Player.transform.position) <= data.GetRangeDetection && 
                    Vector3.Distance(transform.position, Player.transform.position) > data.GetAttackRange)
                {
                    SetState(State.Chase);
                }
                else
                {
                    SetState(State.Attack);
                }
            }
        }

        protected override void Chase()
        {
            if (state == State.Dying)
                return;
            
            if (_chaseCoroutine == null)
                _chaseCoroutine = StartCoroutine(IChase());
            
            IEnumerator IChase()
            {
                while (state == State.Chase)
                {
                    if (Vector3.Distance(transform.position, Player.transform.position) <= data.GetAttackRange)
                    {
                        SetState(State.Attack);
                        _chaseCoroutine = null;
                    }
                
                    _agent.SetDestination(Player.transform.position);
                
                    yield return null;
                }
            }
        }

        private void Shot()
        {
            animator.SetBool("isAttack", true);
            GameObject shot = Instantiate(m_bullet, m_gun.transform.position, Quaternion.identity);
            shot.GetComponent<EnemyBulletController>().SetDirection(Player.transform);
            animator.SetBool("isAttack", false);
        }
    
        private new void Dying()
        {
            base.Dying();
            m_stateSystem.gameObject.SetActive(false);
        }

        public override bool IsMoving()
        {
            return state == State.Chase;
        }

        public override void TakeDamage(float damage = 1, bool isCritical = false)
        {
            base.TakeDamage(damage, isCritical);
        
            if (state == State.Neutral)
                SetState(State.Chase);
        
            if (Healthpoint <= 0)
            {
                SetState(State.Dying);
            }
        }
    }
}