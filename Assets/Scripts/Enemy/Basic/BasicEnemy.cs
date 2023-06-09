using System;
using System.Collections;
using Enemy.Data;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace Enemy.Basic
{
    public class BasicEnemy : EnemyController
    {
        [SerializeField] private EnemyData data;
        [SerializeField] private NavMeshAgent agent;

        [SerializeField] private GameObject m_gun;
        [SerializeField] private GameObject m_bullet;
        [SerializeField] private ParticleSystem m_stateSystem;
        [SerializeField] private Renderer stateRenderer;

        private Coroutine _stateCoroutine;

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

            stateRenderer = m_stateSystem.GetComponent<Renderer>();
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
                    break;
                case State.Chase:
                    Chase();
                    break;
                case State.Attack:
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

            if (!FocusPlayer)
                if (Vector3.Distance(transform.position, Player.transform.position) <= data.GetRangeDetection)
                    FocusPlayer = true;

            if (Vector3.Distance(transform.position, Player.transform.position) <= data.GetRangeDetection &&
                Vector3.Distance(transform.position, Player.transform.position) > data.GetAttackRange)
                SetState(State.Chase);
            else
                SetState(State.Attack);
        }

        protected override void Chase()
        {
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
            Debug.Log("Shooting");
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